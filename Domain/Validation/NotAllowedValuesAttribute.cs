using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.BL.Domain.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
    internal class NotAllowedValuesAttribute : ValidationAttribute
    {
        private readonly ArrayList invalidValues;

        public NotAllowedValuesAttribute(object invalidValue, params object[] invalidValues)
        {
            this.invalidValues = new ArrayList();
            this.invalidValues.Add(invalidValue);
            this.invalidValues.AddRange(invalidValues);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (object invalidValue in this.invalidValues)
            {
                if (Object.Equals(value, invalidValue))
                {
                    return new ValidationResult(String.Format("The value of '{0}' is not allowed"
                    , validationContext.MemberName)
                    , new string[] { validationContext.MemberName });
                }
            }
            return ValidationResult.Success;
        }
    }
}
