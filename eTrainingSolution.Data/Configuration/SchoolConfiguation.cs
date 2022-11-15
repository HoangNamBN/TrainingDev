using eTrainingSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.Data.Configuration
{
    public class SchoolConfiguation : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.ToTable("Schools");
            builder.HasKey(e => e.Id);
            /*
                - mặc định tên cột sẽ là SchoolName.
                - IsRquired(): mặc định giá trị của IsRequired sẽ là true và bắt buộc phải nhập trường thông tin này và kí tự tối đa được nhập là 255 kí tự
                - Đặt lại tên trường mặc định thành "Tên trường học"
            */

            builder.Property(x=> x.SchoolName).IsRequired().HasMaxLength(255).HasColumnName("Tên trường học");
        }
    }
}
