using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.Data.Entities
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
        /// Khóa ngoại: ID trường
        /// </summary>
        public Guid SchoolID { get; set; }
        public School School { get; set;}
    }
}
