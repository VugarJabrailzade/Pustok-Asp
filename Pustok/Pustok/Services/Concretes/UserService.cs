using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using System.Linq;

namespace Pustok.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly PustokDbContext pustokDbContext;

        public UserService(PustokDbContext pustokDbContext)
        {
            this.pustokDbContext = pustokDbContext;
        }

        public User GetCurrentLoggedUser()
        {
            return pustokDbContext.Users.Single(u=> u.Id == -1);
        }


        public string GetFullName(User user)
        {
            return user.Name + " " +user.LastName;
        }
    }
}
