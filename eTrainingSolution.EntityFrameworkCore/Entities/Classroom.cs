namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    public class Classroom
    {
        /// <summary>
        /// ID của lớp học
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// tên lớp học
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// sức chứa của lớp học
        /// </summary>
        public int ClassCapacity { get; set; }
        /// <summary>
        /// ngày thành lập
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        public Faculty Facultys { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
