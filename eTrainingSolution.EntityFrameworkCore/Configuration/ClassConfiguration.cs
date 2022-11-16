using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTrainingSolution.EntityFrameworkCore.Configuration
{
    public class ClassConfiguration : IEntityTypeConfiguration<Classroom>
    {
        /// <summary>
        /// Config bằng cách sử dụng Fluent API
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            // khi chuyển sang sql server thì tên bản sẽ là Classes
            builder.ToTable("Classes");
            // cấu hình khóa chính sẽ là thuộc tính ID
            builder.HasKey(x => x.ID);
            // bắt buộc trường ClassName phải nhập thông tin
            builder.Property(x => x.ClassName).IsRequired(true).IsUnicode();
            // cấu hình khóa ngoại
            builder.HasOne(x => x.Facultys).WithMany(g => g.Classrooms);
        }
    }
}
