using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.WebApp.Areas.Identity.Models.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Role/[action]")]
    public class RoleController : Controller
    {
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

        [TempData]
        public string? StatusMessage { get; set; }

        /// <summary>
        /// HTTPGet đến trang quản lý Role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = _roleManager.Roles.ToList();
            ViewBag.Roles = data;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateAsync(AddRoleModel modelRole)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var newRole = new IdentityRole(modelRole.RoleName);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn đã tạo thành công vai trò mới là: {modelRole.RoleName}";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Không tạo được vai trò mới");
            return View();
        }

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

        [HttpGet("{roleid}")]
        public async Task<IActionResult> EditAsync(string roleID, [Bind("RoleName")] EditRoleModel editRole)
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
            editRole.RoleName = role.Name;
            ModelState.Clear();
            return View(editRole);
        }

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
    }
}
