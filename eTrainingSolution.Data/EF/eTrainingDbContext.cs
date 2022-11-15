using eTrainingSolution.Data.Configuration;
using eTrainingSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eTrainingSolution.Data.EF
{
    public class eTrainingDbContext : DbContext
    {
        /// <summary>
        /// sử dụng để lưu và truy xuất cơ sở dữ liệu
        /// </summary>
        /// <param name="options"></param>
        public eTrainingDbContext(DbContextOptions options) : base(options)
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
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// khai báo các entities
        /// </summary>
        public DbSet<School> Schools { get; set; }
        public DbSet<Faculty> Facultys { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
