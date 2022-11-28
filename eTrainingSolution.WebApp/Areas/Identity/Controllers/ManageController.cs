using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using eTrainingSolution.WebApp.Areas.Identity.Models.Account.Manage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Manage/[action]")]
    public class ManageController : PublicController
    {
        #region Khai báo các dịch vụ cần sử dụng
        public ManageController(SignInManager<UserInfo> signInManager, UserManager<UserInfo> userManager, DB_Context context, RoleManager<IdentityRole> roleManager)
            : base(signInManager, userManager, context, roleManager)
        {
        }
        #endregion

        #region Khai báo Model

        //[BindProperty]
        public ProfileModel profileModel { get; set; }

        //[BindProperty]
        public ChangePassModel changePassModel { get; set; }

        #endregion

        #region Load thông tin user vào model

        public void Load(UserInfo user, bool isUpdate = false)
        {
            if (isUpdate == true)
            {
                profileModel = new ProfileModel
                {
                    UserName = user.UserName,
                    Phone = user.PhoneNumber,
                    Birthday = user.DateOfBirth,
                    Address = user.Address,
                    FullName = user.FullName,
                    StatusMessage = "Bạn đã cập nhật thành công"
                };
            }
            else
            {
                profileModel = new ProfileModel
                {
                    UserName = user.UserName,
                    Phone = user.PhoneNumber,
                    Birthday = user.DateOfBirth,
                    Address = user.Address,
                    FullName = user.FullName,
                };
            }
        }

        #endregion

        #region Index
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(bool isUpdate = false)
        {
            // lấy thông tin user đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(Default.NotificationRole);
            }
            if (isUpdate == true) Load(user, true);
            else Load(user, false);

            return View(profileModel);
        }
        #endregion

        #region Update hồ sơ cá nhân
        [HttpPost]
        public async Task<IActionResult> Update(ProfileModel profileModel)
        {
            bool checkUpdate = false;

            // lấy thông tin user đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(Default.NotificationRole);
            }
            if (!ModelState.IsValid)
            {
                Load(user);
                return RedirectToAction(nameof(Index));
            }

            // Check tính hợp lệ của số điện thoại
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (profileModel.Phone != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, profileModel.Phone);
                if (!setPhoneResult.Succeeded)
                {
                    profileModel.StatusMessage = "Lỗi cập nhật số điện thoại.";
                    return View(profileModel);
                }
            }
            // cập nhật các trường thông tin còn lại
            user.Address = profileModel.Address;
            user.FullName = profileModel.FullName;
            user.PhoneNumber = profileModel.Phone;
            user.DateOfBirth = profileModel.Birthday;

            await _userManager.UpdateAsync(user);
            checkUpdate = true;
            // đăng nhập lại để làm mới Cookie => không nhớ thông tin cũ
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Index), new { isUpdate = checkUpdate });
        }
        #endregion

        #region Update Mật khẩu cá nhân

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            changePassModel = new ChangePassModel();
            // lấy thông tin của user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(Default.NotificationRole);
            }
            return View(changePassModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ChangePassModel changePassModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // lấy thông tin user đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(Default.NotificationRole);
            }

            var changePassword = await _userManager.ChangePasswordAsync(user, changePassModel.OldPassword, changePassModel.NewPassword);
            if (!changePassword.Succeeded)
            {
                foreach (var error in changePassword.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(changePassword);
            }
            await _signInManager.RefreshSignInAsync(user);
            changePassModel.status = "Bạn đã cập nhật mật khẩu thành công";
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
