using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Classes
{
    public class ValidateClass_CreateDate:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null) 
            {
                return new ValidationResult("Bạn cần nhập ngày thành lập của lớp");
            }
            var classInstance = (Classroom)validationContext.ObjectInstance;
            
            var dbContext = (eTrainingDbContext)validationContext?.GetService(typeof(eTrainingDbContext));

            // lấy ngày thành lập của khoa 
            var createDateFaculties = dbContext.Facultys?.FirstOrDefault(m => m.ID == classInstance.FacultyID)?.CreateDate;

            // so sánh 2 thời gian với nhau 
            if(DateTime.Compare((DateTime)createDateFaculties, (DateTime)classInstance.CreateDate) < 0 
                || DateTime.Compare((DateTime)createDateFaculties, (DateTime)classInstance.CreateDate) == 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Ngày thành lập của lớp không được sớm hơn ngày thành lập của Khoa");
        }
    }
}
