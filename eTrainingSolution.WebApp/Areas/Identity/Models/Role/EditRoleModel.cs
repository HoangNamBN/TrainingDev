using eTrainingSolution.EntityFrameworkCore.Validation.Role;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace eTrainingSolution.WebApp.Areas.Identity.Models.Role
{
    public class EditRoleModel
    {
        [Display(Name = "Tên của role")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài {2} đến {1} ký tự")]
        [ValidateRoleName]
        public string? RoleName { get; set; }

    }
}
