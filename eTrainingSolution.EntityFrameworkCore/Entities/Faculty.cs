using eTrainingSolution.EntityFrameworkCore.Validation;
using eTrainingSolution.Shared;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    public class Faculty
    {
        /// <summary>
        /// ID của Khoa
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Tên Khoa
        /// </summary>
        [Required(ErrorMessage ="Bạn cần nhập tên Khoa")]
        [Display(Name = "Khoa")]
        [StringLength(255, ErrorMessage ="Tên Khoa không được vượt quá 255 kí tự")]
        [ValidateFacultyNameIsUnicode] 
        
        public string? FacultyName { get; set; }

        /// <summary>
        /// Sức chứa của Khoa
        /// </summary>
        [Required(ErrorMessage ="Bạn cần nhập số lượng học sinh tối đa của Khoa")]
        [Display(Name = "Số lượng học sinh")]
        [ValidateCapacityOfFaculty]
        public int? CapacityOfFaculty { get; set; }

        /// <summary>
        /// Ngày thành lập
        /// </summary>
        [Required(ErrorMessage ="Bạn cần nhập ngày lập của Khoa")]
        [Display(Name ="Ngày thành lập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        [ValidateCreateDate(ErrorMessage ="Ngày thành lập nhỏ hơn ngày hiện tại")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập người thành lập ra Khoa")]
        [Display(Name = "Người thành lập")]
        [StringLength(255, ErrorMessage = "Người thành lập không được vượt quá 255 kí tự")]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// áp dụng cho fluent api
        /// </summary>
        public School? Schools { get; set; }

        [Display(Name = "Trường")]
        public Guid? SchoolID { get; set; }
        public ICollection<Classroom>? Classrooms { get; set; }
    }
}
