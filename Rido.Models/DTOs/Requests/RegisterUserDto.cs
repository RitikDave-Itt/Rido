using System.ComponentModel.DataAnnotations;

using Rido.Common.Attributes;

namespace Rido.Models.DTOs.Requests
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Phone number is required.")]
        [PhoneNumber(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StrongPassword(ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

    }
}