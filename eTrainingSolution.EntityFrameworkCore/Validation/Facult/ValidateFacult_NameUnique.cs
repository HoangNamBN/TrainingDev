using eTrainingSolution.EntityFrameworkCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.Faculties
{
    public class ValidateFacult_NameUnique : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var facultDB = (Facult)validationContext.ObjectInstance;

            var _dbContext = (DB_Context)validationContext?.GetService(typeof(DB_Context));

            var schoolDB = _dbContext.SchoolET?.ToList();

            foreach (var itemSchool in schoolDB)
            {
                if (itemSchool.ID == facultDB.SchoolID)
                {
                    var facultDbContext = _dbContext.FacultET.Where(m => m.SchoolID == itemSchool.ID).ToList();
                    foreach (var item in facultDbContext)
                    {
                        if (item.ID != facultDB.ID && facultDB.ID != null)
                        {
                            if (value.ToString().ToUpper() == item.Name.ToUpper())
                            {
                                return new ValidationResult("Trường " + itemSchool.Name + " đã tồn tại khoa " + item.Name);
                            }
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
