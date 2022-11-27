using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Role
{
    public class ValidateRole_NameUnique : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Bạn cần chọn quyền muốn sửa");
            }
            var _dbContext = (DB_Context)validationContext?.GetService(typeof(DB_Context));

            var isRole = _dbContext.Roles?.Where(m => m.Name.ToUpper().Contains(value.ToString().ToUpper()));

            if (isRole.Count() != 0)
            {
                return new ValidationResult("Vai trò này đã tồn tại");
            }
            return ValidationResult.Success;
        }
    }
}
