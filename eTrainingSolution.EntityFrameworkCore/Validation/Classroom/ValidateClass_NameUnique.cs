using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Classes
{
    public class ValidateClass_NameUnique : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var classDB = (Classroom)validationContext.ObjectInstance;
            var _dbContext = validationContext?.GetService(typeof(DB_Context)) as DB_Context;

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
            /* Danh sách tồn tại tên được thay đổi và không phải chính nó*/
            var lstClassDb = _dbContext.ClassET?.Where(m => (m.SchoolID == classDB.SchoolID && m.Name.ToUpper().Contains(value.ToString().ToUpper()) && m.ID != classDB.ID)).ToList();
            if(lstClassDb.Count > 0)
            {
                return new ValidationResult("Đã tồn tại");
            }
            return ValidationResult.Success;
        }
    }
}
