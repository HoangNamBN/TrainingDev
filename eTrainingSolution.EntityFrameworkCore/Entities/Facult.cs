using eTrainingSolution.EntityFrameworkCore.Validation.CreateDate;
using eTrainingSolution.EntityFrameworkCore.Validation.Faculties;
using eTrainingSolution.EntityFrameworkCore.Validation.School;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    public class Facult
    {
        /// <summary>
        /// Mã khoa
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Tên Khoa
        /// </summary>
        [Required(ErrorMessage = "Tên Khoa không được để trống")]
        [Display(Name = "Khoa")]
        [StringLength(100, ErrorMessage = "Tên khoa không được vượt quá 100 kí tự")]
        [ValidateFacult_NameUnique]
        public string? Name { get; set; }

        /// <summary>
        /// Sức chứa của Khoa
        /// </summary>
        [Required(ErrorMessage = "Sức chứa của Khoa không được bỏ trống")]
        [Display(Name = "Số lượng học sinh")]
        [ValidateFacult_Capacity]
        public int? Capacity { get; set; }

        /// <summary>
        /// Ngày thành lập
        /// </summary>
        [Required(ErrorMessage = "Ngày thành lập không được để trống")]
        [Display(Name = "Ngày thành lập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Validate_CreateDate(ErrorMessage = "Ngày thành lập nhỏ hơn ngày hiện tại")]
        [ValidateSchool_CreateDate]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        [Required(ErrorMessage = "Người thành lập ra Khoa không được để trống")]
        [Display(Name = "Người thành lập")]
        [StringLength(50, ErrorMessage = "Tên người thành lập không được vượt quá 50 kí tự")]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// áp dụng cho fluent api
        /// </summary>
        [Display(Name = "Trường")]
        public Guid? SchoolID { get; set; }
        public School? Schools { get; set; }

        public ICollection<Classroom>? Classrooms { get; set; }
        public ICollection<UserInfo>? Users { get; set; }
    }
}
