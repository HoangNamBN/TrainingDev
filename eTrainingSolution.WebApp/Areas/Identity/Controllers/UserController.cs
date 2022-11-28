using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using eTrainingSolution.WebApp.Areas.Identity.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/User/[action]")]
    public class UserController : PublicController
    {
        #region Khai báo các dịch vụ sử dụng
        public UserController(SignInManager<UserInfo> signInManager, UserManager<UserInfo> userManager, DB_Context context, RoleManager<IdentityRole> roleManager)
            : base(signInManager, userManager, context, roleManager)
        {
        }
        #endregion

        #region Index
        /// <summary>
        /// Khi thực hiện ấn vào Quản lý thành viên trong Quản lý Role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage)
        {
            var model = new UserModel();
            model.currentPage = currentPage;

            // trả về query
            var query = _userManager.Users.OrderBy(x => x.UserName);

            // tổng số lượng người truy cập = tổng số dòng
            model.totalUser = await query.CountAsync();

            // làm tròn tổng số trang
            model.countPages = (int)Math.Ceiling((double)model.totalUser / model.ITEM_PER_PAGE);

            // Nếu mà trang hiện tại < 1 thì gán cho nó là trang thứ nhất 
            if (model.currentPage < 1)
                model.currentPage = 1;
            // Nếu mà trang hiện tại lớn hơn tổng số trang thì gán cho nó là trang cuối cùng
            if (model.currentPage > model.countPages)
                model.currentPage = model.countPages;

            // skip ví dụ crrentPage là trang số 1 thì sẽ skip 0 phần tử, bỏ ra các phần tử đầu tiên và lấy ra số phần tử ITEM_PER_PAGE
            // trả về một đối tượng mới là UserAndRole
            var query_new = query.Skip((model.currentPage - 1) * model.ITEM_PER_PAGE).Take(model.ITEM_PER_PAGE)
                .Select(u => new UserInfo()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                });

            model.users = await query_new.ToListAsync();

            foreach (var user in model.users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                // các vai trò được nối với nhau bởi dấu phẩy
                user.RoleName = string.Join(", ", roles);
            }
            return View(model);
        }
        #endregion

        #region Thêm role, cập nhật cho user

        /// <summary>
        /// Url: /Identity/AddRole/id
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        [HttpGet("{roleid}")]
        [Authorize(Roles = RoleType.Admin)]
        public async Task<IActionResult> AddRoleAsync(string roleID)
        {
            if (string.IsNullOrEmpty(roleID))
            {
                return NotFound(Default.NotificationRole);
            }
            // tìm user theo id
            var addRole = new AddRoleForUser();
            addRole.user = await _userManager.FindByIdAsync(roleID);
            if (addRole.user.RoleName == RoleType.Admin)
            {
                return RedirectToAction(nameof(Index));
            }
            // lấy ra danh sách các role và lấy ra tên của role
            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roleNames);
            return View(addRole);
        }

        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        [HttpPost("{roleid}")]
        public async Task<IActionResult> AddRoleAsync(string roleID, [Bind("RoleNames")] AddRoleForUser model)
        {
            if (string.IsNullOrEmpty(roleID))
            {
                return NotFound(Default.NotificationRole);
            }
            /* Lấy ra role của user hiện tại */
            model.user = await _userManager.FindByIdAsync(roleID);
            var roleCurrent = (await _userManager.GetRolesAsync(model.user)).ToArray();

            /* Các role cần xóa */
            var roleDelete = roleCurrent.Where(r => !model.RoleNames.Contains(r));

            /* Các role cần cập nhật */
            var roleUpdate = model.RoleNames.Where(e => !roleCurrent.Contains(e));

            /* Truyền danh sách role sang View */
            List<string> roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roles);

            // Thực hiện xóa roles
            if (roleDelete.Count() != 0)
            {
                var resultDelete = await _userManager.RemoveFromRolesAsync(model.user, roleDelete);
                // Nếu mà xóa không thành công
                if (!resultDelete.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Không thể xóa được role");
                    return View(model);
                }
            }

            if (roleUpdate.Count() != 0)
            {
                /* Thực hiện thêm các Role mới */
                foreach (var role in roleUpdate)
                {
                    if (role == RoleType.Admin)
                    {
                        ViewBag.isSuccess = false;
                        return View(model);
                    }
                }
                var resultAdd = await _userManager.AddToRolesAsync(model.user, roleUpdate);
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}