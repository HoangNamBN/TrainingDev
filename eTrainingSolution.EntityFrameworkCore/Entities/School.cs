using eTrainingSolution.EntityFrameworkCore.Validation.CreateDate;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    /// <summary>
    /// 1. attribute configuration [Table("Schools")] // Data Annotation
    /// 2. Fluent Api configuration: đảm bảo class không có các thuộc tính liên quan đến database
    /// </summary>
    public class School
    {
        /// <summary>
        /// School Id
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Tên trường học
        /// </summary>
        [Required(ErrorMessage = "Tên trường không được bỏ trống")]
        [Display(Name = "Trường")]
        [StringLength(230, ErrorMessage = "Tên trường không được vượt quá 230 kí tự")]
        public string? Name { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        [Display(Name = "Địa chỉ")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 kí tự")]
        public string? Address { get; set; }

        /// <summary>
        /// Ngày thành lập
        /// </summary>
        [Required(ErrorMessage = "Ngày thành lập không được để trống")]
        [Display(Name = "Ngày thành lập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Validate_CreateDate(ErrorMessage = "Ngày thành lập không đúng")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// sức chứa của trường học
        /// </summary>
        [Required(ErrorMessage = "Số lượng học sinh của trường không được trống")]
        [Display(Name = "Số lượng học sinh")]
        [Range(0, 1000, ErrorMessage = "Số lượng tối đa chỉ được 1000 học sinh")]
        public int Capacity { get; set; }

        public ICollection<Facult>? Facults { get; set; }
        public ICollection<Classroom>? Classrooms { get; set; }
        public ICollection<UserInfo>? Users { get; set; }
    }
}
