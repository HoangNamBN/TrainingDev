﻿using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTrainingSolution.EntityFrameworkCore.Configuration
{
    public class SchoolConfiguation : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.ToTable("Schools");
            builder.HasKey(e => e.Id);
            /*
                - mặc định tên cột sẽ là SchoolName.
                - IsRquired(): mặc định giá trị của IsRequired sẽ là true và bắt buộc phải nhập trường thông tin này.
            */

            builder.Property(x => x.SchoolName).IsRequired().HasMaxLength(255);
        }
    }
}
