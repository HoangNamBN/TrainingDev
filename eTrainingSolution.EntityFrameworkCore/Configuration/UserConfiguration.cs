using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.EntityFrameworkCore.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Config bằng cách sử dụng Fluent API
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // cấu hình khóa ngoại
            builder.HasOne(x => x.Classrooms).WithMany(g => g.Users);
            builder.HasOne(x => x.Schools).WithMany(g => g.Users);
            builder.HasOne(x => x.Facultys).WithMany(g => g.Users);
        }
    }
}
