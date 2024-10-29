using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Rido.Model.Attributes
{

    public class PhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;

            var regex = new Regex(@"^\+?[1-9]\d{1,14}$");

            if (!string.IsNullOrEmpty(phoneNumber) && !regex.IsMatch(phoneNumber))
            {
                return new ValidationResult("Invalid phone number format.");
            }

            return ValidationResult.Success;
        }
    }
}