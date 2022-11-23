using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using eTrainingSolution.WebApp.Areas.Identity.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Role/[action]")]
    public class RoleController : Controller
    {
        #region Khai báo các dịch vụ sử dụng
        /// <summary>
        /// sử dụng RoleManger để quản lý và đọc các vai trò, các dịch vụ
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly eTrainingDbContext _eTrainingDbContext;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// inject các đối tượng vào trong hàm khởi tạo
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="eTrainingDbContext"></param>
        /// <param name="userManager"></param>
        public RoleController(RoleManager<IdentityRole> roleManager, eTrainingDbContext eTrainingDbContext, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _eTrainingDbContext = eTrainingDbContext;
            _userManager = userManager;
        }
        #endregion

        #region index

        /// <summary>
        /// HTTPGet đến trang quản lý Role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = _roleManager.Roles.OrderBy(data => data.Name).ToList();
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
        public async Task<IActionResult> CreateAsync(AddRoleModel modelRole)
        {
            if (ModelState.IsValid)
            {
                var newRole = new IdentityRole(modelRole.RoleName);
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
            return View(modelRole);
        }

        #endregion

        #region Xóa role

        [Authorize(Roles = RoleType.Admin)]
        [HttpGet("{roleid}")]
        public async Task<IActionResult> DeleteAsync(string roleID)
        {
            if (roleID == null)
            {
                return NotFound("Không tìm thấy vai trò");
            }
            var role = await _roleManager.FindByIdAsync(roleID);
            if (role == null)
            {
                return NotFound("Không tìm thấy Role");
            }
            return View(role);
        }


        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        [HttpPost("{roleid}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmAsync(string roleID)
        {
            if (roleID == null)
            {
                return NotFound("Không tìm thấy role");
            }
            var role = await _roleManager.FindByIdAsync(roleID);

            if (role == null)
            {
                return NotFound("Không tim thấy role");
            }
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
        /// <param name="editRole">model</param>
        /// <returns></returns>
        [HttpGet("{roleid}")]
        [Authorize(Roles = RoleType.Admin)]
        public async Task<IActionResult> EditAsync(string roleID, [Bind("RoleName")] EditRoleModel editRole)
        {
            if (roleID == null)
            {
                return NotFound("Không tìm thấy role");
            }
            // tìm kiếm role
            var role = await _roleManager.FindByIdAsync(roleID);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            editRole.RoleName = role.Name;
            ModelState.Clear();
            return View(editRole);
        }

        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = RoleType.Admin)]
        [HttpPost("{roleid}"), ActionName("Edit")]
        public async Task<IActionResult> EditConfirmAsync(string roleID, [Bind("RoleName")] EditRoleModel editRole)
        {
            if (roleID == null)
            {
                return NotFound("Không tìm thấy role");
            }
            var role = await _roleManager.FindByIdAsync(roleID);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            role.Name = editRole.RoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Không sửa được role");
            return View(editRole);
        }

        #endregion
    }
}