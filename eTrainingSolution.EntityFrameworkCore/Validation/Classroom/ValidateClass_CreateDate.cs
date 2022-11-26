using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Classes
{
    public class ValidateClass_CreateDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var classDb = (Classroom)validationContext.ObjectInstance;

            var _dbContext = (DB_Context)validationContext?.GetService(typeof(DB_Context));

            /* Lấy ngày thành lập của Khoa */
            var createDateFacult = _dbContext.FacultET?.FirstOrDefault(m => m.ID == classDb.FacultID)?.CreateDate;

            int valueCompare = DateTime.Compare((DateTime)createDateFacult, (DateTime)classDb.CreateDate);

            if (valueCompare > 0)
            {
                return new ValidationResult("Ngày thành lập của lớp không được sớm hơn ngày thành lập của Khoa");
            }
            return ValidationResult.Success;
        }
    }
}
