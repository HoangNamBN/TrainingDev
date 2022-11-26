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
                .Select(u => new UserAndRole()
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
            var addRoleForUser = new AddRoleForUser();
            if (string.IsNullOrEmpty(roleID))
            {
                return NotFound(Default.NotificationRole);
            }
            // tìm user theo id
            addRoleForUser.user = await _userManager.FindByIdAsync(roleID);

            addRoleForUser.RoleNames = (await _userManager.GetRolesAsync(addRoleForUser.user)).ToArray<string>();

            // lấy ra danh sách các role và lấy ra tên của role
            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roleNames);
            return View(addRoleForUser);
        }

        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        [HttpPost("{roleid}")]
        public async Task<IActionResult> AddRoleAsync(string roleID, [Bind("RoleNames")] AddRoleForUser addRoleForUser)
        {
            if (string.IsNullOrEmpty(roleID))
            {
                return NotFound(Default.NotificationRole);
            }
            addRoleForUser.user = await _userManager.FindByIdAsync(roleID);
            // lấy ra 1 mảng các role mà user hiện tại đang có
            var roleNamesCurrent = (await _userManager.GetRolesAsync(addRoleForUser.user)).ToArray();

            // So sánh roleNamesCurrent với roleNames nếu mà roles không có các role mà roleNameCurrent hiện tại đang có thì những role đó là những role bị xóa đi
            var roleDelete = roleNamesCurrent.Where(r => !addRoleForUser.RoleNames.Contains(r));
            // những role cần thêm vào (tức là những role tồn tại ở RoleNames nhưng không tồn tại ở roleNamesCurrent
            var roleUpdate = addRoleForUser.RoleNames.Where(e => !roleNamesCurrent.Contains(e));

            // truyền danh sách Role sang cho View 
            List<string> roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roles);

            // Thực hiện xóa roles
            var resultOfDeletingRoles = await _userManager.RemoveFromRolesAsync(addRoleForUser.user, roleDelete);

            // Nếu mà xóa không thành công
            if (!resultOfDeletingRoles.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa được role");
                return View(addRoleForUser);
            }

            // lấy ra danh sách user
            var userDbContext = _context.UserET.ToList();

            foreach (var user in userDbContext)
            {
                // lấy ra quyền hiện tại của user
                var role = await _userManager.GetRolesAsync(user);
                for (int i = 0; i < role.Count; i++)
                {
                    if (role[i] == RoleType.Admin)
                    {
                        foreach (var roleName in roleUpdate)
                        {
                            if (roleName != RoleType.Admin)
                            {
                                var resultOfAddRoles = await _userManager.AddToRolesAsync(addRoleForUser.user, roleUpdate);
                                if (!resultOfAddRoles.Succeeded)
                                {
                                    var resultRole = await _userManager.AddToRolesAsync(addRoleForUser.user, roleNamesCurrent);
                                    ViewBag.isSuccess = false;
                                    return View(addRoleForUser);
                                }
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                var resultOfAddRoles = await _userManager.AddToRolesAsync(addRoleForUser.user, roleNamesCurrent);
                                ViewBag.isSuccess = false;
                                return View(addRoleForUser);
                            }
                        }
                    }
                }
            }
            var resultAddRolesNotAdmin = await _userManager.AddToRolesAsync(addRoleForUser.user, roleUpdate);
            if (!resultAddRolesNotAdmin.Succeeded)
            {
                var resultOfAddRoles = await _userManager.AddToRolesAsync(addRoleForUser.user, roleNamesCurrent);
                ViewBag.isSuccess = false;
                return View(addRoleForUser);
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
