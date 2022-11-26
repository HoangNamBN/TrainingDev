using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTrainingSolution.EntityFrameworkCore.Configuration
{
    public class UserConfigure : IEntityTypeConfiguration<UserInfo>
    {
        /// <summary>
        /// Config bằng cách sử dụng Fluent API
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            // cấu hình khóa ngoại
            builder.HasOne(x => x.Classrooms).WithMany(g => g.Users);
            builder.HasOne(x => x.Schools).WithMany(g => g.Users);
            builder.HasOne(x => x.Facults).WithMany(g => g.Users);
        }
    }
}
