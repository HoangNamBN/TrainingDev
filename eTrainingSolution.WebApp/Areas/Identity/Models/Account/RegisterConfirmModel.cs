using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.WebApp.Areas.Identity.Models.Account
{
    /// <summary>
    /// Model dùng để xác nhận Email
    /// </summary>
    public class RegisterConfirmModel
    {
        [Required(ErrorMessage = "Bạn bắt buộc phải nhập {0}!")]
        [EmailAddress(ErrorMessage = "Bạn nhập sai định dạng Email !")]
        [Display(Name = "Email")]
        /// <summary>
        /// Email dùng để xác nhận sau khi đăng ký
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// đường dẫn dẫn đến việc xác nhận
        /// </summary>
        public string? UrlConfirm { get; set; }
    }
}
