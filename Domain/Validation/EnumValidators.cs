using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.BL.Domain.Validation
{
    public static class EnumValidators
    {
        public static ValidationResult EnumDefinedOnly(object value, ValidationContext validationContext)
        {
            if (!Enum.IsDefined(value.GetType(), value))
            {
                string errorMessage = String.Format("The specified value of '{0}' is not defined"
                                                    , validationContext.MemberName);
                return new ValidationResult(errorMessage, new string[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}
