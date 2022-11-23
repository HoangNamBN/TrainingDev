using eTrainingSolution.EntityFrameworkCore.Entities;

namespace eTrainingSolution.WebApp.Areas.Identity.Models.Users
{
    public class UserModel
    {
        /// <summary>
        /// tổng số lượng người dùng
        /// </summary>
        public int totalUser { get; set; }

        /// <summary>
        /// tổng số trang
        /// </summary>
        public int countPages { get; set; }

        /// <summary>
        /// thiết lập số phần tử hiển thị tại mỗi trang
        /// </summary>
        public int ITEM_PER_PAGE { get; set; } = 5;

        /// <summary>
        /// số trang hiện tại
        /// </summary>
        public int currentPage { get; set; }

        public List<UserAndRole>? users { get; set; }
    }

    public class UserAndRole : User
    {
        public string? RoleName { get; set; }
    }
}
