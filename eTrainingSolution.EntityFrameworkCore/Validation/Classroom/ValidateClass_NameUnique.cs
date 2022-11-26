using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Classes
{
    public class ValidateClass_NameUnique : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var classDB = (Classroom)validationContext.ObjectInstance;
            var _dbContext = (DB_Context)validationContext?.GetService(typeof(DB_Context));

            /* Trường hợp tạo mới */
            if (classDB.ID == null)
            {
                /* Lấy danh sách các lớp theo mã Khoa và mã trường */
                var lstClass = _dbContext.ClassET.Where(m => (m.Name.ToUpper().Contains(value.ToString().ToUpper())
                                                                && m.SchoolID == classDB.SchoolID)).ToList();
                if (lstClass.Count > 0)
                {
                    return new ValidationResult("Đã tồn tại");
                }
                return ValidationResult.Success;

            }
            var lstClassDb = _dbContext.ClassET?.Where(m => m.SchoolID == classDB.SchoolID).ToList();
            foreach (var classDb in lstClassDb)
            {
                if (classDb.ID != classDB.ID)
                {
                    if (value.ToString().ToUpper() == classDb.Name.ToUpper())
                    {
                        return new ValidationResult("Đã tồn tại");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
