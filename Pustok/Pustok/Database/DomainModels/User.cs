using Pustok.Contracts;
using System.Collections.Generic;

namespace Pustok.Database.DomainModels
{
    public class User
    {
         public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public List<UserRole> UserRole { get; set; }
    }
}
