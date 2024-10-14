using System;
using Rido.Data.Enums;

namespace Rido.Data.Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserStatus Status { get; set; }
        public string ProfileImageUrl { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }    


    }
}
