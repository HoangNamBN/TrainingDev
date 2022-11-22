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
    public class eTrainingDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// sử dụng để lưu và truy xuất cơ sở dữ liệu
        /// </summary>
        /// <param name="options"></param>
        public eTrainingDbContext(DbContextOptions<eTrainingDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// hỗ trợ tạo dbcontext gọi đến các Fluent Api configuration
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassConfiguration());
            modelBuilder.ApplyConfiguration(new SchoolConfiguation());
            modelBuilder.ApplyConfiguration(new FacultyConfiguration());
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

        /// <summary>
        /// khai báo các entities thực thể
        /// </summary>
        public DbSet<School> Schools { get; set; }
        public DbSet<Faculty> Facultys { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}