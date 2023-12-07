using Pustok.Contracts;
using Pustok.Database.Abstracts;

namespace Pustok.Database.DomainModels
{
    public class UserRole : IEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
