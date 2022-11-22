using eTrainingSolution.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTrainingSolution.EntityFrameworkCore.Validation
{
    public class ValidateClassNameIsUnicode : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return new ValidationResult("Bạn cần nhập thông tin tên lớp");
            }

            // tạo ra obejct Instance
            var classInstance = (Classroom)validationContext.ObjectInstance;

            var _context = (eTrainingDbContext)validationContext?.GetService(typeof(eTrainingDbContext));

            // lấy thông tin các trường học đang đăng ký
            var schooolDbContext = _context.Schools?.ToList();

            foreach (var itemSchool in schooolDbContext)
            {
                if(itemSchool.Id == classInstance.SchoolID)
                {
                    var facultysDbContext = _context.Facultys.Where(m => m.SchoolID == itemSchool.Id).ToList();
                    foreach (var itemFaculty in facultysDbContext)
                    {
                        var classDbContext = _context.Classrooms?.Where(m => m.FacultyID == itemFaculty.ID).ToList();
                        foreach (var classDb in classDbContext)
                        {
                            if (value.ToString().ToUpper() == classDb.ClassName.ToUpper())
                            {
                                return new ValidationResult("Trường " + itemSchool.SchoolName + " đã tồn tại lớp này tại Khoa " + itemFaculty.FacultyName);
                            }
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
