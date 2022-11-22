using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Loại tài khoản
        /// </summary>
        public int AccountType { get; set; }

        /// <summary>
        /// full name
        /// </summary>
        [Display(Name = "Tên đầy đủ")]
        [StringLength(200, ErrorMessage = "FullName không được vượt quá 200 kí tự")]
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
        public Classroom? Classrooms { get; set; }

    }
}
