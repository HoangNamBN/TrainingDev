using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTrainingSolution.EntityFrameworkCore.Configuration
{
    public class ClassConfigure : IEntityTypeConfiguration<Classroom>
    {
        /// <summary>
        /// Config bằng cách sử dụng Fluent API
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            // Cấu hình tên bảng
            builder.ToTable("Classroom");
            // Cấu hình khóa chính
            builder.HasKey(x => x.ID);
            // Cấu hình trường Name
            builder.Property(x => x.Name).IsRequired(true).IsUnicode(true);
            // Cấu hình khóa ngoại với Faculities và Schools
            builder.HasOne(x => x.Facults).WithMany(g => g.Classrooms);
            builder.HasOne(x => x.Schools).WithMany(g => g.Classrooms);
        }
    }
}
