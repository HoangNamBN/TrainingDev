using eTrainingSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTrainingSolution.Data.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            // cấu hình khóa chính sẽ là thuộc tính ID
            builder.HasKey(x => x.ID);
            // bắt buộc trường ClassName phải nhập thông tin
            builder.Property(x => x.UserName).IsRequired(true);
            // cấu hình khóa ngoại
            builder.HasOne(x => x.Classrooms).WithMany(g => g.Users);
        }
    }
}
