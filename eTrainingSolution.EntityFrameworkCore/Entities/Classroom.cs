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
        [ValidateClassNameIsUnicode]
        public string? ClassName { get; set; }

        /// <summary>
        /// sức chứa của lớp học
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập số lượng học sinh tối đa của một lớp")]
        [Display(Name = "Số lượng học sinh")]
        [ValidateClassesCapacity]
        public int? ClassCapacity { get; set; }

        /// <summary>
        /// ngày thành lập
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập ngày lập của Lớp")]
        [Display(Name = "Ngày thành lập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
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

        [Display(Name = "Khoa")]
        public Guid? FacultyID { get; set; }
        public School? Schools { get; set; }

        [Display(Name = "Trường")]
        public Guid? SchoolID { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
