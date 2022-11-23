using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Role
{
    public class ValidateRoleName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Bạn cần nhập tên cho vai trò của mình");
            }

            var _context = (eTrainingDbContext)validationContext?.GetService(typeof(eTrainingDbContext));

            // lấy ra danh sách các quyền
            var roles = _context.Roles?.ToList();

            foreach (var role in roles)
            {
                if (role.Name.ToUpper() == value.ToString().ToUpper())
                {
                    return new ValidationResult("Vai trò này đã tồn tại");
                }
            }
            return ValidationResult.Success;
        }
    }
}
