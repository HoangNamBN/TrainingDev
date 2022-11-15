using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.Data.Entities
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
        /// <summary>
        /// Id của Khoa
        /// </summary>
        public Faculty Faculty { get;set; }
        public Guid FacultyID { get; set; }
    }
}
