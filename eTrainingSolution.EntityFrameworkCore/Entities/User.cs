using Microsoft.AspNetCore.Identity;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    public class User : IdentityUser
    {
        /// <summary>
        /// full name
        /// </summary>
        public string FullName { get; set; }
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
