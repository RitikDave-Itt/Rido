using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Rido.Common.Attributes
{
    public class StrongPassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Password is required.");
            }

            var hasUpperCase = new Regex(@"[A-Z]");
            var hasLowerCase = new Regex(@"[a-z]");
            var hasNumber = new Regex(@"\d");
            var hasSpecialChar = new Regex(@"[\W_]");
            var isValidLength = password.Length >= 8;

            if (!hasUpperCase.IsMatch(password))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }
            if (!hasLowerCase.IsMatch(password))
            {
                return new ValidationResult("Password must contain at least one lowercase letter.");
            }
            if (!hasNumber.IsMatch(password))
            {
                return new ValidationResult("Password must contain at least one number.");
            }
            if (!hasSpecialChar.IsMatch(password))
            {
                return new ValidationResult("Password must contain at least one special character.");
            }
            if (!isValidLength)
            {
                return new ValidationResult("Password must be at least 8 characters long.");
            }

            return ValidationResult.Success;
        }
    }
}