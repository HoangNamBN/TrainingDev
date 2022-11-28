using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Faculties
{
    public class ValidateFacult_NameUnique : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var facultDB = (Facult)validationContext.ObjectInstance;

            var _dbContext = validationContext?.GetService(typeof(DB_Context)) as DB_Context;

            /* Trường hợp đăng ký một Khoa */
            if(facultDB.ID.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                /* lấy ra danh sách các Khoa theo ID*/
                var lstFacult = _dbContext.FacultET.Where(m => (m.Name.ToUpper().Contains(value.ToString().ToUpper()) && m.SchoolID == facultDB.SchoolID)).ToList();
                if(lstFacult.Count > 0 )
                {
                    return new ValidationResult("Đã tồn tại");
                }
                return ValidationResult.Success;
            }
            var lstfacultDb = _dbContext.FacultET.Where(
                            m => (m.SchoolID == facultDB.ID && m.ID != facultDB.ID && m.Name.ToUpper().Contains(value.ToString().ToUpper()))).ToList();
            if(lstfacultDb.Count > 0)
            {
                return new ValidationResult("Đã tồn tại Khoa");
            }
            return ValidationResult.Success;
        }
    }
}
