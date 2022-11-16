using Microsoft.AspNetCore.Identity;

namespace eTrainingSolution.EntityFrameworkCore.Entities
{
    /// <summary>
    /// IdentityRole đã định nghĩa sẵn các Properties như là ID, Name, Navigate property for the users in this role
    /// </summary>
    public class Role : IdentityRole
    {
        /// <summary>
        /// mô tả
        /// </summary>
        public string Description { get; set; }

    }
}
