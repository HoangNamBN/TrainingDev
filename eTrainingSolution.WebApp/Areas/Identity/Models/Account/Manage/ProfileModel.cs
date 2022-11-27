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

        [Phone(ErrorMessage = "{0} sai định dạng")]
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [MaxLength(50, ErrorMessage = "Họ và tên đầy đủ chỉ được phép dưới 50 kí tự")]
        [Display(Name = "Họ và tên đầy đủ")]
        public string? FullName { get; set; }

        [MaxLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 kí tự")]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày sinh")]
        //[ModelBinder(BinderType =typeof(ConvertDateTime))]
        public DateTime? Birthday { get; set; }

        ///// <summary>
        ///// File dùng để Upload thông tin
        ///// </summary>
        //[Display(Name = "Ảnh cá nhân")]
        //[Required(ErrorMessage = "File ảnh user không được để trống")]
        //[DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png, jpg, jpeg, gif")]
        //public IFormFile? FileUpload { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }
    }
}
