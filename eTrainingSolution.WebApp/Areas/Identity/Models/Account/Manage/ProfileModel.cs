using eTrainingSolution.EntityFrameworkCore.Validation.CreateDate;
using eTrainingSolution.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.WebApp.Areas.Identity.Models.Account.Manage
{
    /// <summary>
    /// dùng để chứa các trường thông tin cần cập nhật cho tài khoản cá nhân
    /// </summary>
    public class ProfileModel
    {
        [Display(Name = "Tên người dùng")]
        public string? UserName { get; set; }

        [Phone(ErrorMessage ="{0} sai định dạng")]
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [MaxLength(50, ErrorMessage = "Họ và tên đầy đủ chỉ được phép dưới 50 kí tự")]
        [Display(Name = "Họ và tên đầy đủ")]
        public string? FullName { get; set; }

        [MaxLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 kí tự")]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày sinh")]
        //[ModelBinder(BinderType =typeof(ConvertDateTime))]
        public DateTime? Birthday { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }
    }
}
