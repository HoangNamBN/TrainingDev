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
        public string SchoolName { get; set; }
        /// <summary>
        /// địa điểm 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// ngày thành lập
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// sức chứa của trường học
        /// </summary>
        public int CapacityOfTheSchool { get; set; }

        public ICollection<Faculty> Faculties { get; set; }
    }
}
