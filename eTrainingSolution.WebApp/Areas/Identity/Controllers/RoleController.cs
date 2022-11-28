using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using eTrainingSolution.WebApp.Areas.Identity.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Role/[action]")]
    public class RoleController : PublicController
    {
        #region Khai báo các dịch vụ sử dụng
        public RoleController(SignInManager<UserInfo> signInManager, UserManager<UserInfo> userManager, DB_Context context, RoleManager<IdentityRole> roleManager)
            : base(signInManager, userManager, context, roleManager)
        {
        }
        #endregion

        #region index

        /// <summary>
        /// HTTPGet đến trang quản lý Role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public async Task<IActionResult> Index()
        {
            var data = await _roleManager.Roles.OrderBy(data => data.Name).ToListAsync();
            return View(data);
        }

        #endregion

        #region Tạo role mới

        /// <summary>
        /// Url /Role/Create
        /// </summary>
        /// <returns>giao diện tạo role mới</returns>
        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = RoleType.Admin)]
        [HttpPost, ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateAsync(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var newRole = new IdentityRole(model.Name);
                var result = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        #endregion

        #region Xóa role

        [Authorize(Roles = RoleType.Admin)]
        [HttpGet("{roleID}")]
        public async Task<IActionResult> DeleteAsync(string roleID)
        {
            if (roleID == null)
            {
                return NotFound(Default.NotificationRole);
            }
            var role = await _roleManager.FindByIdAsync(roleID);
            return View(role);
        }


        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        [HttpPost("{roleID}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmAsync(string roleID)
        {
            if (roleID == null)
            {
                return NotFound(Default.NotificationRole);
            }
            var role = await _roleManager.FindByIdAsync(roleID);

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Không xóa được role");
            return View(role);
        }
        #endregion

        #region Sửa role

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleID">id role được truyền vào từ object ở trang trước đó</param>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpGet("{roleID}")]
        [Authorize(Roles = RoleType.Admin)]
        public async Task<IActionResult> EditAsync(string roleID, [Bind("Name")] RoleModel model)
        {
            if (roleID == null)
            {
                return NotFound(Default.NotificationRole);
            }
            // tìm kiếm role
            var role = await _roleManager.FindByIdAsync(roleID);
            if (role.Name == RoleType.Admin)
            {
                return RedirectToAction(nameof(Index));
            }
            model.Name = role.Name;
            ModelState.Clear();
            return View(model);
        }

        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = RoleType.Admin)]
        [HttpPost("{roleID}"), ActionName("Edit")]
        public async Task<IActionResult> EditConfirmAsync(string roleID, [Bind("Name")] RoleModel model)
        {
            if (roleID == null)
            {
                return NotFound(Default.NotificationRole);
            }
            var role = await _roleManager.FindByIdAsync(roleID);
            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Không sửa được role");
            return View(model);
        }

        #endregion
    }
}