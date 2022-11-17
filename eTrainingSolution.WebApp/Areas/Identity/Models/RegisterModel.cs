using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace eTrainingSolution.WebApp.Areas.Identity.Models
{
    /// <summary>
    /// khi đăng ký thì sẽ yêu cầu người dùng nhập các thông tin như email, password, email vừa cập nhật vào email của user và cũng dùng là UserName
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Required: thông báo lỗi không nhập Email
        /// </summary>
        [Required(ErrorMessage = "Bạn bắt buộc phải nhập {0}!")]
        [EmailAddress(ErrorMessage ="Bạn nhập sai định dạng Email !")]
        [Display(Name = "UserName or Email")]
        public string Email { get; set; }

        /// <summary>
        /// Khi xác thực dữ liệu thì Password và ConfirmPassword phải trùng mật khẩu với nhau
        /// </summary>
        [Required(ErrorMessage = "Bạn bắt buộc phải nhập {0}")]
        [StringLength(100, ErrorMessage ="{0} phải nhập từ {2} đến {1} kí tự !", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Trường thông tin nhập lại Password dùng để so sánh Password với ConfirmPassword
        /// </summary>
        [Required(ErrorMessage = "Bạn chưa nhập {0}")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không chính xác!")]
        public string ConfirmPassword { get; set; }
    }
}
