using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Classes/[action]")]
    public class ClassesController : PublicController
    {

        #region Khai báo dbContext sử dụng
        public ClassesController(SignInManager<UserInfo> signInManager, UserManager<UserInfo> userManager, DB_Context context, RoleManager<IdentityRole> roleManager)
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
            if (isAdmin != true)
            {
                var classDB = _context.ClassET.Include(f => f.Facults).Include(f => f.Schools).Where(m => m.SchoolID == user.SchoolID);
                return View(await classDB.OrderBy(m => m.Name).ToListAsync());

            }
            ViewBag.isAdmin = "Admin";
            var lstClass = from c in _context.ClassET select c;
            if (!string.IsNullOrEmpty(search))
            {
                lstClass = lstClass.Where(m => (m.Facults.Name.Contains(search) || m.Schools.Name.Contains(search)));
                return View(await lstClass.Include(m => m.Schools).Include(m => m.Facults).OrderBy(m => m.Name).ToListAsync());
            }
            return View(await _context.ClassET.Include(m => m.Schools).Include(m => m.Facults).OrderBy(m => m.Name).ToListAsync());
        }

        #endregion

        #region Create Class

        [HttpGet]
        [Authorize(Roles = RoleType.Admin_Manage)]
        public async Task<IActionResult> Create()
        {
            UserInfo user = await getUserET();
            bool isAdmin = await IsAdmin();
            if (isAdmin != true)
            {
                ViewData["FacultID"] = new SelectList(_context.FacultET.Where(m => m.SchoolID == user.SchoolID), Default.ID, Default.FacultName);
                ViewData["SchoolID"] = new SelectList(_context.SchoolET.Where(m => m.ID == user.SchoolID), Default.ID, Default.SchoolName);
                return View();
            }
            ViewData["FacultID"] = new SelectList(_context.FacultET, Default.ID, Default.FacultName);
            ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleType.Admin_Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind(Default.BindClass)] Classroom classes)
        {
            if (ModelState.IsValid)
            {
                classes.ID = Guid.NewGuid();
                _context.Add(classes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultID"] = new SelectList(_context.FacultET, Default.ID, Default.FacultName, classes.FacultID);
            ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName, classes.SchoolID);
            return View(classes);
        }

        #endregion

        #region View thông tin

        [HttpGet]
        [Authorize(Roles = RoleType.Admin_Manage)]
        public async Task<IActionResult> View(Guid? id)
        {
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                var classDb = await _context.ClassET.Include(g => g.Facults).Include(g => g.Schools).FirstOrDefaultAsync(m => m.ID == id);
                return View(classDb);
            }
            return NotFound(Default.NotificationClass);
        }

        #endregion

        #region Sửa thông tin Class

        [HttpGet]
        [Authorize(Roles = RoleType.Admin_Manage)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return NotFound(Default.NotificationClass);
            }
            UserInfo user = await getUserET();
            bool isAdmin = await IsAdmin();
            var classDB = await _context.ClassET.FindAsync(id);
            if (isAdmin != true)
            {
                ViewData["SchoolID"] = new SelectList(_context.SchoolET.Where(m => m.ID == user.SchoolID), Default.ID, Default.SchoolName);
                ViewData["FacultID"] = new SelectList(_context.FacultET.Where(m => m.SchoolID == user.SchoolID), Default.ID, Default.FacultName);
                return View(classDB);
            }
            ViewData["FacultID"] = new SelectList(_context.FacultET, Default.ID, Default.FacultName, classDB.FacultID);
            ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName, classDB.SchoolID);
            return View(classDB);
        }

        /// <summary>
        /// Bind: chỉ định các thuộc tính nào của lớp Model Classroom muốn liên kết
        /// Nếu muốn bỏ bớt các thuộc tính thì thêm Exclude
        /// </summary>
        /// <param name="id"></param>
        /// <param name="classrooms"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleType.Admin_Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind(Default.BindClass)] Classroom classrooms)
        {
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var classDb = _context.ClassET.Find(id);
                        _context.Entry(classDb).CurrentValues.SetValues(classrooms);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["FacultID"] = new SelectList(_context.FacultET, Default.ID, Default.FacultName, classrooms.FacultID);
                ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName, classrooms.SchoolID);
                return View(classrooms);
            }
            return NotFound(Default.NotificationClass);
        }

        #endregion

        #region Xóa thông tin Class

        [HttpGet]
        [Authorize(Roles = RoleType.Admin_Manage)]
        public Task<IActionResult> Delete(Guid? id) => View(id);

        /// <summary>
        /// Do cùng truyền vào tham số là id nên đối với Delete của HttpPost thì đổi tên thành DeleteConfirmed và thêm ActionName
        /// </summary>
        /// <param name="id">id của lớp muốn xóa</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RoleType.Admin_Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                var classDB = await _context.ClassET.FindAsync(id);
                _context.ClassET.Remove(classDB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound(Default.NotificationClass);
        }

        #endregion

        #region Lấy ra danh sách Khoa trả về Jon
        public ActionResult GetFacult(string SchoolID)
        {
            if (!string.IsNullOrEmpty(SchoolID))
            {
                List<SelectListItem> facultJson = _context.FacultET
                    .Where(c => c.SchoolID.ToString() == SchoolID).OrderBy(m => m.Name)
                    .Select(n => new SelectListItem
                    {
                        Value = n.ID.ToString(),
                        Text = n.Name
                    }).ToList();
                return Json(facultJson);
            }
            return null;
        }

        public ActionResult GetClass(string FacultID)
        {
            if (!string.IsNullOrEmpty(FacultID))
            {
                List<SelectListItem> classJson = _context.ClassET
                    .Where(c => c.FacultID.ToString() == FacultID).OrderBy(m => m.Name)
                    .Select(n => new SelectListItem
                    {
                        Value = n.ID.ToString(),
                        Text = n.Name
                    }).ToList();
                return Json(classJson);
            }
            return null;
        }
        #endregion
    }
}
