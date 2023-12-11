using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using System;
using System.Collections.Generic;
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

        public bool IsCurrentUserInRole(params string[] roles)
        {
            return roles.Any(r => httpContextAccessor.HttpContext.User.IsInRole(r));

        }


        public UserService(PustokDbContext pustokDbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.pustokDbContext = pustokDbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        private User GetCurrentLoggedUser()
        {
            var currentUser = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "Id").Value;

            return pustokDbContext.Users.
                Include(x=> x.UserRole).
                Single(u=> u.Id == Convert.ToInt32(currentUser));
        }

        public bool IsUserSeeded(User user)
        {
            return user.Id < 0;
        }


        public string GetFullName(User user)
        {
            return user.Name + " " + user.LastName;
        }

        public string GetCurrentUserFullName()
        {
            return GetFullName(CurrentUser);
        }

        public List<User> GetWholeStaff()
        {

            var staff = pustokDbContext.Users.Where(v=> v.UserRole.Any(u=> 
                        u.Role == Role.Moderator ||
                        u.Role == Role.Admin || 
                        u.Role == Role.SuperAdmin || 
                        u.Role == Role.SMM)).ToList();

            return staff;
        }
    }
}
