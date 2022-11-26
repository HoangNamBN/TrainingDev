using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/School/[action]")]
    public class SchoolController : PublicController
    {
        #region Kế thừa BaseController
        public SchoolController(SignInManager<UserInfo> signInManager, UserManager<UserInfo> userManager, DB_Context context, RoleManager<IdentityRole> roleManager)
            : base(signInManager, userManager, context, roleManager)
        {
        }
        #endregion

        #region Index

        [HttpGet]
        [Authorize(Roles = RoleType.Admin_Manage)]
        public async Task<IActionResult> Index(string search)
        {
            UserInfo user = await getUserET();
            bool isAdmin = await IsAdmin();
            if (!isAdmin)
            {
                var schoolDB = _context.SchoolET.Where(m => m.ID == user.SchoolID);
                return View(await schoolDB.ToListAsync());
            }
            ViewBag.isAdmin = "Admin";
            var schoolSearch = from school in _context.SchoolET select school;
            if (!string.IsNullOrEmpty(search))
            {
                schoolSearch = schoolSearch.Where(x => x.Name.Contains(search));
                return View(await schoolSearch.OrderBy(m => m.Name).ToListAsync());
            }
            return View(await _context.SchoolET.OrderBy(m => m.Name).ToListAsync());
        }

        #endregion

        #region Create School

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind(Default.BindSchool)] School school)
        {
            if (ModelState.IsValid)
            {
                school.ID = Guid.NewGuid();
                _context.Add(school);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }

        #endregion

        #region Edit School

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                var schoolDB = await _context.SchoolET.FindAsync(id);
                return View(schoolDB);
            }
            return NotFound(Default.NotificationSchool);
        }

        [HttpPost]
        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind(Default.BindSchool)] School school)
        {
            if (id == school.ID)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(school);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(school);
            }
            return NotFound("Không thể sửa thông tin trường học");
        }

        #endregion

        #region Delete School

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public Task<IActionResult> Delete(Guid? id) => Edit(id);

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null) return NotFound();
            // tìm kiếm thông tin trường học theo id
            var schoolDB = await _context.SchoolET.FindAsync(id);
            if (schoolDB != null)
            {
                // lấy ra danh sách các Khoa theo id trường học
                var facultDB = await _context.FacultET.Where(m => m.SchoolID == id).ToListAsync();
                if (facultDB != null)
                {
                    // Duyệt Khoa để lấy ra thông tin của lớp học
                    foreach (var f in facultDB)
                    {
                        // lấy ra danh sách lớp của Khoa
                        var classDB = await _context.ClassET.Where(m => m.FacultID == f.ID).ToListAsync();
                        if (classDB != null)
                        {
                            var userDb = await _context.UserET.Where(m => m.SchoolID == id).ToListAsync();
                            _context.RemoveRange(classDB);
                            if (userDb != null)
                            {
                                _context.RemoveRange(userDb);
                            }
                        }
                        _context.Remove(f);
                    }
                }
                _context.Remove(schoolDB);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region View thông tin

        [HttpGet]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        public Task<IActionResult> View(Guid? id) => Edit(id);
        #endregion
    }
}
