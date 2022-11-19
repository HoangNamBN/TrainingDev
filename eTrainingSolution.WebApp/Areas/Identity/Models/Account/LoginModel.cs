using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace eTrainingSolution.WebApp.Areas.Identity.Models.Account
{
    public class LoginModel
    {
        /// <summary>
        /// Email dùng để đăng nhập
        /// </summary>
        [Required(ErrorMessage = "Bạn bắt buộc phải nhập {0}!")]
        [EmailAddress(ErrorMessage = "Bạn nhập sai định dạng Email !")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Password nhập vào như lúc đăng ký
        /// </summary>
        [Required(ErrorMessage = "Bạn bắt buộc phải nhập thông tin {0}")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} phải nhập từ {2} đến {1} kí tự !", MinimumLength = 8)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// nhớ thông tin đăng nhập
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
