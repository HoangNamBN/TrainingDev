using eTrainingSolution.EntityFrameworkCore.Configuration;
using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eTrainingSolution.EntityFrameworkCore
{
    /// <summary>
    /// DbContext
    ///     - được hiểu đơn giản là cầu nối giữa lớp domain thực thể với CSDL
    ///     - Chịu tương tác với dữ liệu như là một đối tượng
    /// IdentityDbContext:
    ///     - có sẵn các bảng về Identity UserRoles, Roles, UserLogins, ...
    /// </summary>
    public class DB_Context : IdentityDbContext<UserInfo, IdentityRole, string>
    {
        /// <summary>
        /// sử dụng để lưu và truy xuất cơ sở dữ liệu
        /// </summary>
        /// <param name="options"></param>
        public DB_Context(DbContextOptions<DB_Context> options) : base(options)
        {

        }

        /// <summary>
        /// hỗ trợ tạo dbcontext gọi đến các Fluent Api configuration
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassConfigure());
            modelBuilder.ApplyConfiguration(new SchoolConfigure());
            modelBuilder.ApplyConfiguration(new FacultConfigure());
            modelBuilder.ApplyConfiguration(new UserConfigure());
            base.OnModelCreating(modelBuilder);
            // loại bỏ tiền tố AspNet trước AspNetUserRole, ...
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // lấy tên của các table
                var tableName = entityType.GetTableName();
                // kiểm tra xem nếu tên table bắt đầu bằng AspNet thì set lại tên cho table bằng cách cắt chuỗi từ kí tự ở vị trí 0 đến 6
                if (tableName.StartsWith("AspNet"))
                    entityType.SetTableName(tableName.Substring(6));
            }
        }

        /// <summary>
        /// cấu hình: thiết lập chuỗi kết nối
        /// được gọi khi 1 đối tượng DbContext được gọi
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        #region Khai báo các Entities
        /// <summary>
        /// khai báo các entities thực thể
        /// </summary>
        public DbSet<School> SchoolET { get; set; }
        public DbSet<Facult> FacultET { get; set; }
        public DbSet<Classroom> ClassET { get; set; }
        public DbSet<UserInfo> UserET { get; set; }
        #endregion
    }
}