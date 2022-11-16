using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTrainingSolution.EntityFrameworkCore.Configuration
{
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            // khi chuyển sang sql server thì tên bản sẽ là Classes
            builder.ToTable("Faculty");
            // cấu hình khóa chính sẽ là thuộc tính ID
            builder.HasKey(x => x.ID);
            // bắt buộc trường ClassName phải nhập thông tin
            builder.Property(x => x.FacultyName).IsRequired().IsUnicode();
            // cấu hình khóa ngoại
            builder.HasOne(x => x.Schools).WithMany(g => g.Faculties);
        }
    }
}
