using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.WebApp.Areas.Identity.Models.Users
{
    public class AddRoleForUser
    {
        public User? user { get; set; }

        /// <summary>
        /// khai báo một mảng string chứa các quyền mà người dùng có thể có
        /// </summary>
        [Display(Name ="Các role được gán cho user")]
        public string[]? RoleNames { get; set; }
    }
}
