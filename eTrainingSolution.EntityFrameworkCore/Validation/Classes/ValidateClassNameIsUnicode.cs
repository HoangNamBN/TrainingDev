using eTrainingSolution.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Classes
{
    public class ValidateClassNameIsUnicode : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Bạn cần nhập thông tin tên lớp");
            }

            // tạo ra obejct Instance
            var classInstance = (Classroom)validationContext.ObjectInstance;
            var _context = (eTrainingDbContext)validationContext?.GetService(typeof(eTrainingDbContext));

            // lấy ra danh sách Khoa của trường hiện tại
            var faculitiesDb = _context.Facultys.Where(m => m.SchoolID == classInstance.SchoolID);
            foreach (var itemFaculty in faculitiesDb.ToList())
            {
                var classDbContext = _context.Classrooms?.Where(m => m.FacultyID == itemFaculty.ID).ToList();
                foreach (var classDb in classDbContext)
                {
                    if (classDb.ID != classInstance.ID && classInstance.ID != null)
                    {
                        if (value.ToString().ToUpper() == classDb.ClassName.ToUpper())
                        {
                            return new ValidationResult("Đã tồn tại");
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
