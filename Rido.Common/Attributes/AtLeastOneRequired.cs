using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Attributes
{
    public class AtLeastOneRequired : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public AtLeastOneRequired(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var propertyName in _propertyNames)
            {
                var propertyValue = validationContext.ObjectType.GetProperty(propertyName)?.GetValue(validationContext.ObjectInstance, null);
                if (!string.IsNullOrEmpty(propertyValue as string))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
