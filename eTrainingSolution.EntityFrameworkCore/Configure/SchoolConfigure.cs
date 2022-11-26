using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTrainingSolution.EntityFrameworkCore.Configuration
{
    public class SchoolConfigure : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.ToTable("Schools");
            builder.HasKey(e => e.ID);
            /*
                - mặc định tên cột sẽ là SchoolName.
                - IsRquired(): mặc định giá trị của IsRequired sẽ là true và bắt buộc phải nhập trường thông tin này.
            */

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        }
    }
}
