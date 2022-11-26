using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.School
{
    internal class ValidateSchool_CreateDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var facultDB = (Facult)validationContext.ObjectInstance;

            var _dbContext = (DB_Context)validationContext?.GetService(typeof(DB_Context));

            /* Lấy ngày thành lập của trường */
            var createDateSchool = _dbContext.SchoolET?.FirstOrDefault(m => m.ID == facultDB.SchoolID)?.CreateDate;

            int valueCompare = DateTime.Compare((DateTime)createDateSchool, (DateTime)facultDB.CreateDate);

            if (valueCompare > 0)
            {
                return new ValidationResult("Ngày thành lập của Khoa không được sớm hơn ngày thành lập của Trường");
            }
            return ValidationResult.Success;
        }
    }
}
