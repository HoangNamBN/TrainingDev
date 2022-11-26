using System.ComponentModel.DataAnnotations;

namespace eTrainingSolution.EntityFrameworkCore.Validation.CreateDate
{
    public class Validate_CreateDate : ValidationAttribute
    {
        /// <summary>
        /// validate trường ngày thành lập (ngày thành lập phải lớn hơn ngày tạo)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object? value)
        {
            if (value == null) return false;
            DateTime dateTime = Convert.ToDateTime(value);
            return dateTime <= DateTime.Now;
        }
    }
}
