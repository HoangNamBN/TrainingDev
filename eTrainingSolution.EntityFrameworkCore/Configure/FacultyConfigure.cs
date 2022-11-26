using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTrainingSolution.EntityFrameworkCore.Configuration
{
    public class FacultConfigure : IEntityTypeConfiguration<Facult>
    {
        public void Configure(EntityTypeBuilder<Facult> builder)
        {
            // khi chuyển sang sql server thì tên bản sẽ là Classes
            builder.ToTable("Facults");
            // cấu hình khóa chính sẽ là thuộc tính ID
            builder.HasKey(x => x.ID);
            // bắt buộc trường Name phải nhập thông tin
            builder.Property(x => x.Name).IsRequired().IsUnicode(true);
            // cấu hình khóa ngoại
            builder.HasOne(x => x.Schools).WithMany(g => g.Facults);
        }
    }
}
