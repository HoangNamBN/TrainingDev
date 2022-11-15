namespace eTrainingSolution.Data.Entities
{
    public class User
    {
        /// <summary>
        /// id của user
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// tên user
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// full name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// email của user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// địa chỉ quê quán
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// có muốn xóa hay không
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        public Classroom Classrooms { get; set; }

    }
}
