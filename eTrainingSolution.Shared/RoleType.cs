namespace eTrainingSolution.Shared
{
    public class RoleType
    {
        /// <summary>
        /// quyền quản trị viên:
        ///     - Được tham gia vào tất cả các chức năng trên hệ thống
        ///     - Phân quyền cho phép ví dụ User thuộc trường nào thì chỉ được xem thông tin của trường đó
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Giáo viên thì được xem thông tin của trường, của lớp và được sửa thông tin của chính mình
        /// </summary>
        public const string Teacher = "Teacher";

        /// <summary>
        /// Học sinh thì chỉ được xem thông tin các lớp và sửa thông tin của chính mình
        /// </summary>
        public const string Student = "Student";

        /// <summary>
        /// member thì chỉ được sửa thông tin của chính mình khi nào được cấp các quyền khác thì mới được sử dụng các chức năng tùy thuộc vào quyền hạn
        /// </summary>
        public const string Member = "Member";

        // => đã đổi thành chỉ có Admin mới xem được thông tin của trường, lớp, khoa, ... còn lại chỉ được sửa thông tin của chính mình
    }
}