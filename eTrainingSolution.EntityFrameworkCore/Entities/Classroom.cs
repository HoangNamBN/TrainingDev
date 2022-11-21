using eTrainingSolution.EntityFrameworkCore.Validation;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    public class Classroom
    {
        /// <summary>
        /// ID của lớp học
        /// </summary>
        public Guid? ID { get; set; }

        /// <summary>
        /// tên lớp học
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập tên lớp")]
        [Display(Name = "Lớp")]
        [StringLength(255, ErrorMessage = "Tên Lớp không được vượt quá 255 kí tự")]
        public string? ClassName { get; set; }

        /// <summary>
        /// sức chứa của lớp học
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập số lượng học sinh tối đa của một lớp")]
        [Display(Name = "Số học sinh tối đa của một lớp")]
        [ValidateClassesCapacity]
        public int? ClassCapacity { get; set; }

        /// <summary>
        /// ngày thành lập
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập ngày lập của Lớp")]
        [Display(Name = "Ngày thành lập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ValidateCreateDate(ErrorMessage ="Ngày thành lập nhỏ hơn ngày hiện tại")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// người tạo
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập người thành lập ra Khoa")]
        [Display(Name = "Người thành lập")]
        [StringLength(255, ErrorMessage = "Người thành lập không được vượt quá 255 kí tự")]
        public string? CreatedBy { get; set; }

        public Faculty? Facultys { get; set; }

        [Display(Name = "Thuộc Khoa")]
        public Guid? FacultyID { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
