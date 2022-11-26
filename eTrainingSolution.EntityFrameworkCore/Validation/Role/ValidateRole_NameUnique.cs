using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Role
{
    public class ValidateRole_NameUnique : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var _dbContext = (DB_Context)validationContext?.GetService(typeof(DB_Context));

            var checkRoleExists = _dbContext.Roles?.ToList();
            foreach (var i in checkRoleExists)
            {
                if (i.Name.ToUpper() == value.ToString().ToUpper())
                {
                    return new ValidationResult("Vai trò này đã tồn tại");
                }
            }
            return ValidationResult.Success;
        }
    }
}
