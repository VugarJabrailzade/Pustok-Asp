using Microsoft.AspNetCore.Http;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using System;
using System.Linq;

namespace Pustok.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly PustokDbContext pustokDbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private User currentUser = null;

        public User CurrentUser
        {
            get 
            { 
                return currentUser ??= GetCurrentLoggedUser(); 
            }
        }

        public bool IsAuthenticated
        {
            get { return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated; }
        }


        public UserService(PustokDbContext pustokDbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.pustokDbContext = pustokDbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        private User GetCurrentLoggedUser()
        {
            var currentUser = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "Id").Value;

            return pustokDbContext.Users.Single(u=> u.Id == Convert.ToInt32(currentUser));
        }


        public string GetFullName(User user)
        {
            return user.Name + " " +user.LastName;
        }
    }
}
