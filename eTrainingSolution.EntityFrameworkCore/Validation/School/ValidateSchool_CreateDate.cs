using eTrainingSolution.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.EntityFrameworkCore.Validation.School
{
    internal class ValidateSchool_CreateDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Bạn cần nhập ngày thành lập của khoa");
            }
            var faculitiesInstance = (Faculty)validationContext.ObjectInstance;

            var dbContext = (eTrainingDbContext)validationContext?.GetService(typeof(eTrainingDbContext));

            // lấy ngày thành lập của trường học 
            var createDateSchool = dbContext.Schools?.FirstOrDefault(m => m.Id == faculitiesInstance.SchoolID)?.CreateDate;

            // so sánh 2 thời gian với nhau 
            if (DateTime.Compare((DateTime)createDateSchool, (DateTime)faculitiesInstance.CreateDate) < 0
                || DateTime.Compare((DateTime)createDateSchool, (DateTime)faculitiesInstance.CreateDate) == 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Ngày thành lập của khoa không được sớm hơn ngày thành lập của trường");
        }
    }
}
