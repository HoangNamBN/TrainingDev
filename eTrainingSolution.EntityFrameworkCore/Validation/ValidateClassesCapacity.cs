using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation
{
    public class ValidateClassesCapacity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Bạn cần nhập số lượng học sinh tối đa của lớp");
            // Tạo ra instance của classes
            var classesCapacity = (Classroom)validationContext.ObjectInstance;
            // return instance của service
            var _context = (eTrainingDbContext)validationContext?.GetService(typeof(eTrainingDbContext));

            if(_context.Classrooms.FirstOrDefault(m => m.ID == classesCapacity.ID) != null)
            {
                return ValidationResult.Success;
            }

            // nếu mà chưa chọn Khoa cho lớp học
            if(classesCapacity.FacultyID == null)
            {
                return new ValidationResult("Bạn cần chọn Khoa cho lớp học đăng ký");
            }

            // lấy số lượng học sinh tối đa của Khoa theo mã ID
            var capactityFaculty = _context.Facultys?.FirstOrDefault(m => m.ID == classesCapacity.FacultyID)?.CapacityOfFaculty;

            // lấy tổng số lượng học sinh các lớp
            var capactityClasses =  _context.Classrooms.Where(x => x.FacultyID == classesCapacity.FacultyID).ToList() ?? new List<Classroom>();
            var sum_capactityClasses = capactityClasses.Sum(x => x.ClassCapacity);

            // tính số lượng sinh viên còn lại mà lớp có thể đăng ký được trong một Khoa
            var capactity_Subtraction = capactityFaculty - sum_capactityClasses;
            if(capactity_Subtraction > 0)
            {
                return (classesCapacity?.ClassCapacity > capactity_Subtraction? new ValidationResult($"Khoa còn đủ chỗ cho {(int)capactity_Subtraction} học sinh") : ValidationResult.Success);
            }
            return new ValidationResult("Khoa đã đủ sinh viên");
        }
    }
}
