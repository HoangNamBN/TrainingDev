using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using eTrainingSolution.WebApp.Areas.Identity.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    /// <summary>
    /// AllowAnonymous: Không cần login vần vào được
    /// [Area("Identity")]: chỉ ra tên của Area là Identity
    /// Route: Đường dẫn chung tác động lên các action
    /// </summary>
    [Area("Identity")]
    [Route("/Account/[action]")]
    public class AccountController : PublicController
    {
        #region Khai báo các dịch vụ sử dụng
        public AccountController(SignInManager<UserInfo> signInManager, UserManager<UserInfo> userManager, DB_Context context, RoleManager<IdentityRole> roleManager)
            : base(signInManager, userManager, context, roleManager)
        {
        }
        #endregion

        #region Register
        /// <summary>
        /// AllowAnonymous: Không cần đăng nhập cũng vào được hàm này
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            /* Lấy danh sách user */
            List<UserInfo> users = GetListUser();
            ViewBag.isExitUser = users.Count == 0 ? false : true;

            /* truyền dữ liệu từ Controller sang View */
            ViewData["SchoolID"] = new SelectList(_context.SchoolET, Default.ID, Default.SchoolName);
            ViewData["FacultID"] = new SelectList(_context.FacultET, Default.ID, Default.FacultName);
            ViewData["ClassID"] = new SelectList(_context.ClassET, Default.ID, Default.ClassName);

            return View();
        }

        /// <summary>
        /// ValidateAntiForgeryToken: chống giả mạo một request không phải từ chính website
        /// </summary>
        /// <param name="returnUrl">/Account/Register</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserInfo model, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var userName = model.Email.Substring(0, model.Email.LastIndexOf('@'));
                var user = new UserInfo
                {
                    UserName = userName,
                    Email = model.Email,
                };

                /* trả về đối tượng resultIdentity */
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    /* Chưa có ai đăng ký => gán quyền Admin */
                    if (model.SchoolID == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(RoleType.Admin));
                        user.RoleName = RoleType.Admin;
                        await _userManager.AddToRoleAsync(user, RoleType.Admin);
                    }
                    else
                    {
                        user.SchoolID = model.SchoolID;
                        user.FacultID = model.FacultID;
                        user.ClassID = model.ClassID;
                        await _roleManager.CreateAsync(new IdentityRole(RoleType.Member));
                        user.RoleName = RoleType.Member;
                        await _userManager.AddToRoleAsync(user, RoleType.Member);
                    }

                    /* Yêu cầu xác thực tài khoản: Giá trị RequireConfirmedAccount = false mặc định */
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("RegisterConfirmation",
                            new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        /* isPersistent: false không cần đăng nhập lại, thông tin được lưu ở Cookie */
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                /* Đưa Errors vào ModelState hiển thị ở html helper */
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Dẫn đến view RigisterConfirm để xác nhận đăng ký
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RegisterConfirmation(string email, string? returnUrl)
        {
            return View();
        }

        /// <summary>
        /// confirm email
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="code"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userID, string code, string? returnUrl)
        {
            return View();
        }

        #endregion

        #region Login

        /// <summary>
        /// gọi đến View login
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet("/login/")]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Thực hiện nhập các thông tin => đăng nhập
        /// </summary>
        /// <param name="model">model chứa các thông tin được nhập vào</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost("/login/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            /* Kiểm tra nếu user đăng nhập rồi thì chuyển đến index */
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("Index");
            }
            /* Check xem thông tin model có hợp lệ hay không */
            if (ModelState.IsValid)
            {
                /* lấy tên user */
                var userName = model.Email.Substring(0, model.Email.LastIndexOf('@'));
                /* lockoutOnFailure: true kích hoạt khóa tài khoản khi nhập sai quá số lần */
                var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, lockoutOnFailure: true);

                /* Login không thành công*/
                if (!result.Succeeded)
                {
                    /* Lấy thông tin user theo Email */
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
                    }
                }
                /* Nhập sai quá 3 lần thì chuyển đến Lockout */
                if (result.IsLockedOut) return View("Lockout");
                if (result.Succeeded) return Redirect(returnUrl);
                else
                {
                    ModelState.AddModelError(string.Empty, "Đăng nhập không thành công");
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion

        #region Truy cập bị từ chối
        /// <summary>
        /// Khi không đủ quyền thì sẽ trả về trang này
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

        #region Logout
        /// <summary>
        /// Thực hiện hành động khi ấn vào button Logout
        /// </summary>
        /// <returns>Url:Identity/Account/logout/</returns>
        [HttpPost("/logout/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // thực hiện gọi logout một cách đơn giản
            await _signInManager.SignOutAsync();
            // xóa cookie
            Response.Cookies.Delete("aspToken");
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        #endregion

        #region Trả về kết quả lọc dữ liệu select
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

        public ActionResult GetClass(string SchoolID, string FacultID)
        {
            if (!string.IsNullOrEmpty(FacultID) && !string.IsNullOrEmpty(SchoolID))
            {
                List<SelectListItem> classJson = _context.ClassET
                    .Where(c => (c.FacultID.ToString() == FacultID && c.SchoolID.ToString() == SchoolID)).OrderBy(m => m.Name)
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
