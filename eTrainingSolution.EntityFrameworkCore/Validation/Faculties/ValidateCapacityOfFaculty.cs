using System.ComponentModel.DataAnnotations;
using eTrainingSolution.EntityFrameworkCore.Entities;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Faculties
{
    public class ValidateCapacityOfFaculty : ValidationAttribute
    {
        /// <summary>
        /// Ghi đè IsValid để tạo ra customValidate
        /// </summary>
        /// <param name="value">giá trị truyền vào</param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // nếu mà không có giá trị được truyền vào
            if (value == null)
            {
                return new ValidationResult("Bạn cần nhập số lượng học sinh của Khoa");
            }
            // khởi tạo ra một đối tượng
            var capacityOfFaculty = (Faculty)validationContext.ObjectInstance;

            // trả về null hoặc một instance của service
            var _context = (eTrainingDbContext)validationContext?.GetService(typeof(eTrainingDbContext));

            // trả về kết quả thành công nếu như mà tìm ra phần tử đầu tiên của Faculty
            if (_context.Facultys.FirstOrDefault(m => m.ID == capacityOfFaculty.ID) != null)
            {
                return ValidationResult.Success;
            }

            // Nếu như mà chưa chọn trường  học
            if (capacityOfFaculty.SchoolID == null)
            {
                return new ValidationResult("Bạn chưa chọn trường");
            }

            // lấy sức chứa tối đa của trường theo mã ID
            var capcityOfSchool = _context.Schools?.FirstOrDefault(m => m.Id == capacityOfFaculty.SchoolID)?.CapacityOfTheSchool;

            // Lấy ra số học sinh tối đa của trường đã đăng ký
            var all_capacticyOfFaculty = _context.Facultys.Where(x => x.SchoolID == capacityOfFaculty.SchoolID).ToList() ?? new List<Faculty>();
            var sum_capacticyOfFaculty = all_capacticyOfFaculty.Sum(x => x.CapacityOfFaculty);

            // tính số lượng sinh viên mà Khoa có thể đăng ký được tại trường đã đăng ký
            var capacticy_Subtraction = capcityOfSchool - sum_capacticyOfFaculty;

            if (capacticy_Subtraction > 0)
            {
                return capacityOfFaculty?.CapacityOfFaculty > capacticy_Subtraction ? new ValidationResult($"Trường còn đủ chỗ cho {(int)capacticy_Subtraction} học sinh") : ValidationResult.Success;
            }
            return new ValidationResult("Trường đã đủ 1000 học sinh");
        }
    }
}
