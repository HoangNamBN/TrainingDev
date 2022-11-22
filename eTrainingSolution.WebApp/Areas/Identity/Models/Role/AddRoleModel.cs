using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.WebApp.Areas.Identity.Models.Role
{
    public class AddRoleModel
    {
        [Required(ErrorMessage = "Bạn phải nhập vai trò")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
        [Display(Name = "Vai trò")]
        public string? RoleName { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }
        //[Display(Name = "Mô tả")]
        //[MaxLength(255, ErrorMessage = "Mô tả chỉ được tối đa 255 kí tự")]
        //public string? Describe { get; set; }
    }
}
