using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.WebApp.Areas.Identity.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    /// <summary>
    /// AllowAnonymous: xác nhận quyền người dùng không login được vẫn có quyền truy cập page
    /// [Area("Identity")]: chỉ ra tên của Area là Identity
    /// Route: dùng để xác định một đường dẫn chung và đường dẫn này có tác động lên tất cả action
    /// </summary>
    [Area("Identity")]
    [Route("/Account/[action]")]
    public class AccountController : Controller
    {
        #region Khai báo các dịch vụ sử dụng

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// đăng kí các dịch vụ
        /// </summary>
        /// <param name="signInManager">Quản lý việc login, logout, ...</param>
        /// <param name="userManager">Quản lý việc thêm sửa xóa tài khoản</param>
        /// <param name="logger">Viết ra log</param>
        /// <param name="emailSender">dùng để xác thực email</param>
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<AccountController> logger, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }
        #endregion

        #region Register
        /// <summary>
        /// lấy ra đường dẫn chỉ đến form đăng ký
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        // cho phép người dùng user không cần đăng nhâp cũng truy cập được action này
        [AllowAnonymous] 
        public IActionResult Register(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Khi submit thì Register nhận được sẽ kiểm tra tất cả các dữ liệu được nhập vào.
        /// </summary>
        /// <param name="returnUrl">/Account/Register</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        // chống giả mạo một request không phải từ chính website
        // cơ chế là khi gửi request Post lên nó sẽ kiểm tra token có tồn tại hay không. Nếu cookie bị thiếu hoặc giá trị không đúng thị nó sẽ không xử lý request này
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            // Kiểm tra xem có trường thông tin nào không hợp lệ hay không xác thực
            if (ModelState.IsValid)
            {
                var userName = model.Email.Substring(0, model.Email.LastIndexOf('@'));
                var user = new User { UserName = userName, Email = model.Email };
                // trả về đối tượng IdentityResult
                var result = await _userManager.CreateAsync(user, model.Password);

                // Nếu mà thêm thành công
                if (result.Succeeded)
                {
                    _logger.LogInformation("Đã tạo user mới!");

                    // phát sinh Token dựa theo thông tin của User để xác thực Email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // convert Token thành Encode để đính kèm trên Url
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // Url sẽ có dạng một cách đầy đủ như sau: https://localhost:5001/Identity/Account/ConfirmEmail?userId=giá trị&code=giá trị&returnUrl=
                    // Nếu mà thiết lập đường dẫn ở bên front end thì đường dẫn Url sẽ có dạng: https://localhost:5001/confirm-email?userId=giá trị&code=giá trị&returnUrl=
                    var callbackUrl = Url.Page(
                        // thiết lập gọi đến trang ConfirmEmail
                        "/Account/ConfirmEmail", pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl }, protocol: Request.Scheme);

                    // gửi tới email của người dùng đã đăng ký
                    await _emailSender.SendEmailAsync(model.Email, "Xác thực địa chỉ Email",
                        $"Bạn đã đăng ký tài khoản thành công! <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> bấm vào đây </a> để kích hoạt tài khoản");

                    // if mà yêu cầu xác nhận tài khoản trước khi đăng nhập thì sẽ chuyển hướng sang trang RegisterConfirmation
                    if (_userManager.Options.SignIn.RequireConfirmedAccount) // mặc định giá trị của RequireConfirmedAccount là false
                    {
                        return RedirectToAction("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        // không cần phải xác nhận mà đăng nhập luôn
                        // nếu để isPersistent: true thì lần sau khi truy cập vào sẽ không cần phải đăng nhập lại, thông tin tài khoản sẽ được lưu ở cookie
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                // đưa lỗi thêm user và ModelState để hiển thị ở html heleper
                foreach (var error in result.Errors)
                {
                    // thêm một message lỗi cụ thể vào danh sách Errors với 1 key
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
        /// <param name="loginModel">model chứa các thông tin được nhập vào</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost("/login/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;
            // Nếu người dùng đã login rồi thì trả về trang Index
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("Index");
            }

            if (ModelState.IsValid)
            {
                // đặt lockoutOnFailure: true cho phép kích hoạt khóa tài khoản khi lỗi mật khẩu quá số lần
                var resultLogin = await _signInManager.PasswordSignInAsync(
                        loginModel.Email.Substring(0, loginModel.Email.LastIndexOf('@')), loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);

                // Nếu như tài khoản đăng nhập nhập sai quá 3 lần
                if (resultLogin.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản của bạn đã bị xóa do sai quá 3 lần");
                    return View("Lockout");
                }

                // Nếu login không thành công
                if (!resultLogin.Succeeded)
                {
                    // tìm thông tin user theo email
                    var user = await _userManager.FindByEmailAsync(loginModel.Email);
                    //Nếu user tồn tại
                    if (user != null)
                    {
                        resultLogin = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);
                    }
                }
                if (resultLogin.Succeeded)
                {
                    // ghi vào log là đã đăng nhập thành công và Redire vào trang tương ứng
                    _logger.LogInformation(loginModel.Email.Substring(0, loginModel.Email.LastIndexOf('@')) + "đã đăng nhập thành công");
                    return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không đăng nhập được");
                    return View(loginModel);
                }
            }
            return View(loginModel);
        }
        #endregion

        #region Truy cập bị từ chối
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
            _logger.LogInformation("Người dùng đã đăng xuất");
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        #endregion
    }
}
