using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using eTrainingSolution.WebApp.Areas.Identity.Models.Role;
using eTrainingSolution.WebApp.Areas.Identity.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/School/[action]")]
    public class SchoolController : BaseController
    {

        #region Kế thừa BaseController
        public SchoolController(SignInManager<User> signInManager, 
            UserManager<User> userManager, eTrainingDbContext eTrainingDbContext,
            RoleManager<IdentityRole> roleManager) : base(signInManager, userManager, eTrainingDbContext, roleManager)
        {

        }

        #endregion

        #region Index

        [HttpGet]
        [Authorize(Roles = RoleType.Manage + "," + RoleType.Admin)]
        public async Task<IActionResult> Index(string search)
        {
            User user = await getUserEntities();
            if(user != null)
            {
                bool checkAdmin = await IsAdmin();
                if (checkAdmin)
                {
                    ViewBag.isAdmin = "Admin";
                    var schoolSearch = from school in _eTrainingDbContext.Schools select school;
                    if (!string.IsNullOrEmpty(search))
                    {
                        schoolSearch = schoolSearch.Where(x => x.SchoolName.Contains(search));
                        return View(await schoolSearch.ToListAsync());
                    }
                    return View(await _eTrainingDbContext.Schools.ToListAsync());
                }

                var schoolDBContext = _eTrainingDbContext.Schools.Where(m => m.Id == user.SchoolID);
                if (schoolDBContext == null)
                {
                    return Problem("null");
                }
                return View(await schoolDBContext.ToListAsync());
            }
            return View();
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
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind("Id, SchoolName, Address, CreateDate, CapacityOfTheSchool")] School school)
        {
            if (ModelState.IsValid)
            {
                school.Id = Guid.NewGuid();
                _eTrainingDbContext.Add(school);
                await _eTrainingDbContext.SaveChangesAsync();
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
            if (id == null || _eTrainingDbContext.Schools == null)
            {
                return NotFound();
            }
            var school = await _eTrainingDbContext.Schools.FindAsync(id);
            return View(school);
        }

        [HttpPost]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, SchoolName, Address, CreateDate, CapacityOfTheSchool")] School school)
        {
            if (id != school.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // bản chất của _eTrainingDbContext.Update là kiểm tra xem Enity có null không và sau đó gán là Modified
                    _eTrainingDbContext.Update(school);
                    await _eTrainingDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }

        #endregion

        #region Delete School

        [HttpGet]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _eTrainingDbContext.Schools == null)
            {
                return NotFound();
            }
            var school = await _eTrainingDbContext.Schools.FirstOrDefaultAsync(m => m.Id == id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null || _eTrainingDbContext.Schools == null)
            {
                return NotFound();
            }
            // tìm kiếm thông tin trường học theo id
            var schoolDb = await _eTrainingDbContext.Schools.FindAsync(id);
            if (schoolDb != null)
            {
                // lấy ra danh sách các Khoa theo id trường học
                var facultiesDb = await _eTrainingDbContext.Facultys.Where(m => m.SchoolID == id).ToListAsync();
                if (facultiesDb != null)
                {
                    // Duyệt Khoa để lấy ra thông tin của lớp học
                    foreach (var faculties in facultiesDb)
                    {
                        // lấy ra danh sách lớp của Khoa
                        var classDB = await _eTrainingDbContext.Classrooms
                            .Where(m => m.FacultyID == faculties.ID).ToListAsync();
                        if (classDB != null)
                        {
                            var userDb = await _eTrainingDbContext.Users.Where(m => m.SchoolID == id).ToListAsync();
                            _eTrainingDbContext.RemoveRange(classDB);
                            if(userDb!= null)
                            {
                                _eTrainingDbContext.RemoveRange(userDb);
                            }
                        }
                        _eTrainingDbContext.Remove(faculties);
                    }
                }
                _eTrainingDbContext.Remove(schoolDb);
            }
            await _eTrainingDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region View thông tin

        [HttpGet]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        public async Task<IActionResult> View(Guid? id)
        {
            if (id == null || _eTrainingDbContext.Schools == null)
            {
                return NotFound();
            }
            var school = await _eTrainingDbContext.Schools.FirstOrDefaultAsync(m => m.Id == id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        #endregion
    }
}
