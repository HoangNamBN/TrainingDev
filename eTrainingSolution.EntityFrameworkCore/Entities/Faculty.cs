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
        public string FacultyName { get; set; }
        /// <summary>
        /// Sức chứa của Khoa
        /// </summary>
        public int CapacityOfFaculty { get; set; }
        /// <summary>
        /// Ngày thành lập
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// áp dụng cho fluent api
        /// </summary>
        public School Schools { get; set; }

        public ICollection<Classroom> Classrooms { get; set; }
    }
}
