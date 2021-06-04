using System;

namespace Demo.Models.Models.Auth
{
    public class UserModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }
        
        public string Email { get; set; }
        
        public string PasswordHash { get; set; }
        
        public DateTime RegisterDate { get; set; }
    }
}