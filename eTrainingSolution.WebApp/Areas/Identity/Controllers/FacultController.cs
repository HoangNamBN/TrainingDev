using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Facult/[action]")]
    public class FacultController : PublicController
    {
        #region Khai báo dbContext sử dụng
        public FacultController(SignInManager<UserInfo> signInManager, UserManager<UserInfo> userManager, DB_Context context, RoleManager<IdentityRole> roleManager)
            : base(signInManager, userManager, context, roleManager)
        {
        }
        #endregion

        #region Index
        [Authorize(Roles = RoleType.Admin_Manage)]
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            UserInfo user = await getUserET();
            bool isAdmin = await IsAdmin();
            if (!isAdmin)
            {
                var facultDB = _context.FacultET.Include(f => f.Schools).Where(m => m.SchoolID == user.SchoolID).OrderBy(m => m.Name);
                return View(await facultDB.ToListAsync());
            }
            ViewBag.isAdmin = "Admin";
            var facultSearch = from facult in _context.FacultET select facult;
            if (!string.IsNullOrEmpty(search))
            {
                facultSearch = facultSearch.Where(x => x.Schools.Name.Contains(search));
                return View(await facultSearch.Include(f => f.Schools).OrderBy(m => m.Name).ToListAsync());
            }
            return View(await _context.FacultET.Include(f => f.Schools).OrderBy(m => m.Name).ToListAsync());
        }

        #endregion

        #region Tạo mới một Khoa

        [Authorize(Roles = RoleType.Admin_Manage)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserInfo user = await getUserET();
            bool isAdmin = await IsAdmin();
            if (!isAdmin)
            {
                ViewData["SchoolID"] = new SelectList(_context.SchoolET.Where(m => m.ID == user.SchoolID), Default.ID, Default.SchoolName);
                return View();
            }
            ViewBag.isAdmin = "Admin";
            ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleType.Admin_Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind(Default.BindFacult)] Facult facult)
        {
            if (ModelState.IsValid)
            {
                facult.ID = Guid.NewGuid();
                _context.Add(facult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName, facult.SchoolID);
            return View(facult);
        }

        #endregion

        #region Sửa thông tin Khoa

        [Authorize(Roles = RoleType.Admin_Manage)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return NotFound(Default.NotificationFacult);
            }
            /* Lấy thông tin Khoa theo ID */
            var facultDB = await _context.FacultET.FindAsync(id);

            UserInfo user = await getUserET();
            bool isAdmin = await IsAdmin();
            if (!isAdmin)
            {
                ViewData["SchoolID"] = new SelectList(_context.SchoolET
                    .Where(m => m.ID == user.SchoolID), Default.ID, Default.SchoolName, facultDB.SchoolID);
                return View(facultDB);
            }
            ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName, facultDB.SchoolID);
            return View(facultDB);
        }

        [HttpPost]
        [Authorize(Roles = RoleType.Admin_Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind(Default.BindFacult)] Facult facult)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var facultDb = _context.FacultET.Find(id);
                    _context.Entry(facultDb).CurrentValues.SetValues(facult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName, facult.SchoolID);
            return View(facult);
        }

        #endregion

        #region Xóa thông tin của Khoa

        [HttpGet]
        [Authorize(Roles = RoleType.Admin_Manage)]
        public async Task<IActionResult> Delete(Guid? id) => await View(id);

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RoleType.Admin_Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                /* lấy Khoa muốn xóa */
                var facultDB = await _context.FacultET.FindAsync(id);

                /* lấy ra danh sách class theo mã Khoa*/
                var classDB = await _context.ClassET.Where(m => m.FacultID == id).ToListAsync();
                if (classDB != null)
                {
                    _context.ClassET.RemoveRange(classDB);
                }
                _context.FacultET.Remove(facultDB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return NotFound(Default.NotificationFacult);
        }

        #endregion

        #region Xem thông tin của Khoa

        [HttpGet]
        [Authorize(Roles = RoleType.Admin_Manage)]
        public async Task<IActionResult> View(Guid? id)
        {
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                var facultDB = await _context.FacultET.Include(g => g.Schools).FirstOrDefaultAsync(m => m.ID == id);
                return View(facultDB);
            }
            return NotFound(Default.NotificationFacult);
        }

        #endregion
    }
}
