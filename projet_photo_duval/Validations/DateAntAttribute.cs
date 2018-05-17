using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projet_photo_duval.Validations
{
    public class DateAntAttribute : ValidationAttribute
    {
       // private DateTime _date;

        public DateAntAttribute()
            : base()
        {
           // _date = date;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime convertedDate = Convert.ToDateTime(value);
                if (convertedDate < DateTime.Now)
                {
                    var msgErreur = FormatErrorMessage(validationContext.DisplayName);

                    return new ValidationResult(msgErreur);
                }
            }

            return ValidationResult.Success;
        }
    }
}