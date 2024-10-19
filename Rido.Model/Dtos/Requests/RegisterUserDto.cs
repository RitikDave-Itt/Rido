using Rido.Common.Attributes;
using Rido.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Models.Requests
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]

        public Gender Gender { get; set; }



        [Required(ErrorMessage = "Phone number is required.")]
        [PhoneNumber(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

      


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password is required.")]
        [StrongPassword(ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string PasswordHash { get; set; }

    }
}
