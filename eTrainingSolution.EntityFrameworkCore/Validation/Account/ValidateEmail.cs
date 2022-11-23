using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Account
{
    public class ValidateEmail : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Email không được để trống");
            }

            var _context = (eTrainingDbContext)validationContext?.GetService(typeof(eTrainingDbContext));

            // lấy danh sách
            var listUser = _context.Users?.ToList();

            foreach (var user in listUser)
            {
                if (value.Equals(user.Email))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("Bạn đã nhập sai email");
        }
    }
}
