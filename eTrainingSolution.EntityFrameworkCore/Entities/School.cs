using eTrainingSolution.Shared;
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
        public Guid Id { get; set; }
        /// <summary>
        /// Tên trường học
        /// </summary>
        [Required(ErrorMessage = "Bạn phải nhập tên trường")]
        [Display(Name = "Tên trường")]
        [StringLength(255, ErrorMessage = "Tên trường không được vượt quá 255 kí tự")]
        public string? SchoolName { get; set; }
        /// <summary>
        /// địa điểm 
        /// </summary>
        [Required(ErrorMessage = "Bạn phải nhập địa chỉ của trường học")]
        [Display(Name = "Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 kí tự")]
        public string? Address { get; set; }
        /// <summary>
        /// ngày thành lập
        /// </summary>
        [Required(ErrorMessage = "Bạn phải nhập ngày thành lập")]
        [Display(Name = "Ngày thành lập trường")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ValidateCreateDate(ErrorMessage = "Ngày đang chọn chưa đến")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// sức chứa của trường học
        /// </summary>
        [Required(ErrorMessage = "Bạn cần nhập sức chứa của trường học")]
        [Display(Name = "Số lượng học sinh tối đa")]
        [Range(0, 1000, ErrorMessage = "Số lượng học sinh tôi đa chỉ được phép là 1000 học sinh")]
        public int CapacityOfTheSchool { get; set; }

        public ICollection<Faculty>? Faculties { get; set; }
    }
}
