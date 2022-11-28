using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Faculties
{
    public class ValidateFacult_Capacity : ValidationAttribute
    {
        /// <summary>
        /// Ghi đè IsValid để tạo ra customValidate
        /// </summary>
        /// <param name="value">giá trị truyền vào</param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var facultDB = (Facult)validationContext.ObjectInstance;

            var _dbContext = validationContext?.GetService(typeof(DB_Context)) as DB_Context;

            /* Lấy sức chứa của trường theo mã ID*/
            var capacitySchool = _dbContext.SchoolET?.FirstOrDefault(m => m.ID == facultDB.SchoolID)?.Capacity;

            /* Lấy ra số lượng học sinh tối đa của Khoa*/
            List<Facult> capacityFacult;
            if (facultDB.ID.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                capacityFacult = _dbContext.FacultET.Where(x => x.SchoolID == facultDB.SchoolID).ToList()
                                        ?? new List<Facult>();
            }
            else
            {
                capacityFacult = _dbContext.FacultET.Where(x => (x.SchoolID == facultDB.SchoolID && x.ID != facultDB.ID)).ToList()
                                        ?? new List<Facult>();
            }

            var sumCapactiyFacult = capacityFacult.Sum(x => x.Capacity);

            /* Số lượng học sinh còn có thể đăng ký */
            var capacityRemain = capacitySchool - sumCapactiyFacult;

            if (capacityRemain > 0)
            {
                return facultDB?.Capacity > capacityRemain
                    ? new ValidationResult($"Trường còn đủ chỗ cho {(int)capacityRemain} học sinh") : ValidationResult.Success;
            }
            return new ValidationResult("Trường đã đủ 1000 học sinh");
        }
    }
}
