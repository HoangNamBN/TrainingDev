using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace eTrainingSolution.WebApp.Areas.Identity.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bạn bắt buộc phải nhập thông tin {0}")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn bắt buộc phải nhập thông tin {0}")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
