using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.WebApp.Areas.Identity.Models.Account.Manage
{
    public class ChangePassModel
    {
        /// <summary>
        /// Password hiện tại
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập giá trị cho Password hiện tại")]
        [DataType(DataType.Password)]
        [Display(Name = "Password hiện tại")]
        public string? OldPassword { get; set; }

        /// <summary>
        /// Password mới
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập Password mới")]
        [StringLength(50, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password mới")]
        public string? NewPassword { get; set; }

        /// <summary>
        /// Nhập lại Password
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại password mới")]
        [Compare("NewPassword", ErrorMessage = "Password phải giống nhau.")]
        [Required(ErrorMessage = "Password nhập lại không được để trống")]
        public string? ConfirmPassword { get; set; }

        public string? status { get; set; }
    }
}
