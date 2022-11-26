using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Account
{
    public class ValidateEmail : ValidationAttribute
    {
        /// <summary>
        /// Validate khi nhập Email đăng nhập
        /// </summary>
        /// <param name="value">Giá trị Email được nhập vào</param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var _DbContext = (DB_Context)validationContext?.GetService(typeof(DB_Context));

            /* lấy ra user tồn tại Email có giá trị value */
            var isEmailExists = _DbContext.Users?.Where(m => m.Email.Contains((string)value));

            if (isEmailExists.Count() > 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Email nhập sai");
        }
    }
}
