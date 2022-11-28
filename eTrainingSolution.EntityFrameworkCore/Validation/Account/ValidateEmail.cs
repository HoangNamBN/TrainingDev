using eTrainingSolution.EntityFrameworkCore.Entities;
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
            var _DbContext = validationContext?.GetService(typeof(DB_Context)) as DB_Context;

            /* lấy ra user tồn tại Email có giá trị value */
            var isEmailExists = _DbContext.UserET?.Where(m => m.Email.Contains(value.ToString()));

            if (isEmailExists.Count() > 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Email nhập sai");
        }
    }
}
