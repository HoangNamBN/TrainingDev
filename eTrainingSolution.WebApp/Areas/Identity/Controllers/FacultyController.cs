using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using eTrainingSolution.WebApp.Areas.Identity.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Linq;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Faculty/[action]")]
    public class FacultyController : BaseController
    {
        #region Khai báo dbContext sử dụng

        public FacultyController(SignInManager<User> signInManager, UserManager<User> userManager, eTrainingDbContext eTrainingDbContext, 
            RoleManager<IdentityRole> roleManager) : base(signInManager, userManager, eTrainingDbContext, roleManager)
        {
        }

        #endregion

        #region Index
        [Authorize(Roles = RoleType.Admin+","+RoleType.Manage)]
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            // lấy thông tin user đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            // lấy ra quyền hiện tại của user 
            var roleName = await _userManager.GetRolesAsync(user);
            for (int i = 0; i < roleName.Count; i++)
            {
                if (roleName[i] == RoleType.Admin)
                {
                    ViewBag.isAdmin = "Admin";
                    // lấy danh sách các Khoa của các trường
                    var faculyDbSearch = from faculty in _eTrainingDbContext.Facultys select faculty;

                    if (!string.IsNullOrEmpty(search))
                    {
                        faculyDbSearch = faculyDbSearch.Where(x => x.Schools.SchoolName.Contains(search));
                        return View( await faculyDbSearch.Include(f => f.Schools).ToListAsync());
                    }
                    return View(await _eTrainingDbContext.Facultys.Include(f => f.Schools).ToListAsync());
                }
            }
            var eTrainDbContext = _eTrainingDbContext.Facultys.Include(f => f.Schools).Where(m => m.SchoolID == user.SchoolID);
            return View(await eTrainDbContext.ToListAsync());
        }

        #endregion

        #region Tạo mới một Khoa

        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // lấy thông tin user đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            // lấy ra quyền hiện tại của user 
            var roleName = await _userManager.GetRolesAsync(user);
            for (int i = 0; i < roleName.Count; i++)
            {
                if (roleName[i] == RoleType.Admin)
                {
                    ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName");
                    return View();
                }
            }
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools.Where(m => m.Id == user.SchoolID), "Id", "SchoolName");
            return View();
        }

        /// <summary>
        /// Bind("ID, FacultyName, CapacityOfFaculty, CreateDate, CreatedBy, SchoolID"): tạo ra 1 list item add vào BindAtrribute
        /// </summary>
        /// <param name="faculty"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind("ID, FacultyName, CapacityOfFaculty, CreateDate, CreatedBy, SchoolID")] Faculty faculty)
        {
            if(ModelState.IsValid)
            {
                faculty.ID = Guid.NewGuid();
                _eTrainingDbContext.Add(faculty);
                await _eTrainingDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", faculty.SchoolID);
            return View(faculty);
        }

        #endregion

        #region Xem thông tin của Khoa

        [HttpGet]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        public async Task<IActionResult> View(Guid? id)
        {
            if(id == null || _eTrainingDbContext.Facultys == null)
            {
                return NotFound();
            }
            // câu lệnh query để truy vấn giữa ID của bảng Facultys với id của bảng Schools
            var faculitiesDbContext = await _eTrainingDbContext.Facultys.Include(g => g.Schools).FirstOrDefaultAsync(m => m.ID == id);
            if(faculitiesDbContext == null)
            {
                return NotFound();
            }
            return View(faculitiesDbContext);
        }

        #endregion

        #region Sửa thông tin Khoa

        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _eTrainingDbContext.Facultys == null)
            {
                return NotFound();
            }
            // tìm Khoa theo ID
            var faculitiesDbContext = await _eTrainingDbContext.Facultys.FindAsync(id);
            if(faculitiesDbContext == null)
            {
                return NotFound();
            }

            // lấy thông tin user đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            // lấy ra quyền hiện tại của user 
            var roleName = await _userManager.GetRolesAsync(user);
            for (int i = 0; i < roleName.Count; i++)
            {
                if (roleName[i] == RoleType.Admin)
                {
                    ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", faculitiesDbContext.SchoolID);
                    return View(faculitiesDbContext);
                }
            }
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools.Where(m => m.Id == user.SchoolID), "Id", "SchoolName", faculitiesDbContext.SchoolID);
            return View(faculitiesDbContext);
        }

        [HttpPost]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind("ID, FacultyName, CapacityOfFaculty, CreateDate, CreatedBy, SchoolID")] Faculty faculty)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var facultyesDb = _eTrainingDbContext.Facultys.Find(id);
                    _eTrainingDbContext.Entry(facultyesDb).CurrentValues.SetValues(faculty);
                    await _eTrainingDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", faculty.SchoolID);
            return View(faculty);
        }

        #endregion

        #region Xóa thông tin của Khoa

        [HttpGet]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var faculitiesDbContext = await _eTrainingDbContext.Facultys.Include(m => m.Schools).FirstOrDefaultAsync(_ => _.ID == id);
            if(faculitiesDbContext == null)
            {
                return NotFound();
            }
            return View(faculitiesDbContext);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RoleType.Admin + "," + RoleType.Manage)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            if(_eTrainingDbContext.Facultys == null)
            {
                return Problem("Entity set null");
            }
            // Lấy thông tin Khoa theo ID
            var faculytiesDbContext = await _eTrainingDbContext.Facultys.FindAsync(id);
            if(faculytiesDbContext!= null)
            {
                // Tìm ra danh sách các lớp trong Khoa được chọn
                var classDbContext = await _eTrainingDbContext.Classrooms.Where(m=> m.FacultyID == id).ToListAsync();
                if(classDbContext!= null)
                {
                    _eTrainingDbContext.Classrooms.RemoveRange(classDbContext);
                }
                _eTrainingDbContext.Facultys.Remove(faculytiesDbContext);
            }
            await _eTrainingDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
