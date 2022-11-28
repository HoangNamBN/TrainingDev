using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    public class UserInfo : IdentityUser
    {
        /// <summary>
        /// full name
        /// </summary>
        [Display(Name = "Họ và tên")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 kí tự")]
        public string? FullName { get; set; }

        /// <summary>
        /// ngày sinh
        /// </summary>
        [Display(Name = "Sinh ngày")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// địa chỉ quê quán
        /// </summary>
        [Display(Name = "Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 kí tự")]
        public string? Address { get; set; }

        /// <summary>
        /// có muốn xóa hay không
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// password
        /// </summary>
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} phải nhập từ {2} đến {1} kí tự !", MinimumLength = 8)]
        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }

        /// <summary>
        /// ngày tạo
        /// </summary>
        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// người tạo
        /// </summary>
        [Display(Name = "Người tạo")]
        [StringLength(255, ErrorMessage = "Tên người tạo chỉ tối đa 255 kí tự")]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Trường thông tin nhập lại Password dùng để so sánh Password với ConfirmPassword
        /// </summary>
        [StringLength(100, ErrorMessage = "{0} phải nhập từ {2} đến {1} kí tự !", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không chính xác!")]
        public string? ConfirmPassword { get; set; }

        /// <summary>
        /// Thêm một cột tên quyền vào để phục vụ cho việc xóa danh sách user có quyền
        /// </summary>
        public string? RoleName { get; set; }

        public string? ImageName { get; set; }

        /// <summary>
        /// Lớp
        /// </summary>
        public Guid? ClassID { get; set; }
        public Classroom? Classrooms { get; set; }

        /// <summary>
        /// Khoa
        /// </summary>
        public Guid? FacultID { get; set; }
        public Facult? Facults { get; set; }

        /// <summary>
        /// Trường học
        /// </summary>
        public Guid? SchoolID { get; set; }
        public School? Schools { get; set; }
    }
}
