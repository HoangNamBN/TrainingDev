using eTrainingSolution.EntityFrameworkCore.Validation.Classes;
using eTrainingSolution.EntityFrameworkCore.Validation.CreateDate;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    public class Classroom
    {
        /// <summary>
        /// Mã lớp học
        /// </summary>
        public Guid? ID { get; set; }

        /// <summary>
        /// Tên lớp học
        /// </summary>
        [Required(ErrorMessage = "Tên lớp không được để trống")]
        [Display(Name = "Lớp học")]
        [StringLength(50, ErrorMessage = "Tên Lớp không được vượt quá 50 kí tự")]
        [ValidateClass_NameUnique]
        public string? Name { get; set; }

        /// <summary>
        /// Sức chứa của lớp học
        /// </summary>
        [Required(ErrorMessage = "Số lượng học sinh không được trống")]
        [Display(Name = "Số lượng học sinh")]
        [ValidateClass_Capacity]
        public int? Capacity { get; set; }

        /// <summary>
        /// Ngày thành lập
        /// </summary>
        [Required(ErrorMessage = "Ngày thành lập chưa được chọn")]
        [Display(Name = "Ngày thành lập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Validate_CreateDate(ErrorMessage = "Ngày thành lập chưa đến")]
        [ValidateClass_CreateDate]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        [Required(ErrorMessage = "Người tạo chưa được nhập thông tin")]
        [Display(Name = "Người thành lập")]
        [StringLength(50, ErrorMessage = "Tên người tạo không vượt quá 50 kí tự")]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Thuộc tính dùng để tạo khóa ngoại
        /// </summary>
        [Display(Name = "Khoa")]
        public Guid? FacultID { get; set; }
        public Facult? Facults { get; set; }

        [Display(Name = "Trường")]
        public Guid? SchoolID { get; set; }
        public School? Schools { get; set; }

        public ICollection<UserInfo>? Users { get; set; }
    }
}
