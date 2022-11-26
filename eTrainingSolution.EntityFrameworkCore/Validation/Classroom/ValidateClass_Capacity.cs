using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Classes
{
    public class ValidateClass_Capacity : ValidationAttribute
    {
        /// <summary>
        /// Validate số lượng học sinh của lớp
        /// </summary>
        /// <param name="value">Số lượng học sinh</param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var classDB = (Classroom)validationContext.ObjectInstance;
            var _dbContext = (DB_Context)validationContext?.GetService(typeof(DB_Context));

            /* Lấy số lượng học sinh của Khoa dựa theo mã Khoa */
            var capacityFacult = _dbContext.FacultET?.FirstOrDefault(m => m.ID == classDB.FacultID)?.Capacity;

            /* Lấy tổng số học sinh của các lớp trong Khoa */
            List<Classroom> capactyClass;
            if (classDB.ID == null)
            {
                capactyClass = _dbContext.ClassET.Where(x => x.FacultID == classDB.FacultID).ToList() ?? new List<Classroom>();
            }
            else
            {
                capactyClass = _dbContext.ClassET.Where(x => (x.FacultID == classDB.FacultID && x.ID != classDB.ID)).ToList() ?? new List<Classroom>();
            }
            var sumCapacityClass = capactyClass.Sum(x => x.Capacity);
            /* Tính số lượng học sinh lại của Khoa đó */
            var capacityRemain = capacityFacult - sumCapacityClass;
            if (capacityRemain > 0)
            {
                return classDB?.Capacity > capacityRemain ? new ValidationResult($"Khoa còn đủ chỗ cho {(int)capacityRemain} học sinh")
                    : ValidationResult.Success;
            }
            return new ValidationResult("Đã đủ số lượng sinh viên");
        }
    }
}
