using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cmp;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using Pustok.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pustok.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly PustokDbContext pustokDbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private static  List<UserConnectionViewModel> userConnection;
        private User currentUser = null;

        static UserService()
        {
            userConnection = new List<UserConnectionViewModel>();

        }

        public UserService(PustokDbContext pustokDbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.pustokDbContext = pustokDbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

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

        public List<string> GetUserConnection(int userId)
        {
            return userConnection.SingleOrDefault(uc => uc.UserId == userId)?.ConnectionIds ?? new List<string>();
        }

        public void AddCurrentUserConnection(string connectionId)
        {

            var connectionIds = userConnection.SingleOrDefault(uc => uc.UserId == CurrentUser.Id)?.ConnectionIds;

            if(connectionIds == null)
            {
                userConnection.Add(new UserConnectionViewModel
                {
                    UserId = CurrentUser.Id,
                    ConnectionIds = new List<string> { connectionId }
                });

            }
            else
            {
                connectionIds.Add(connectionId);
            }
        }


        public void RemoveCurrentUserConnection(string connectionId)
        {
            var connectionIds = userConnection.SingleOrDefault(uc => uc.UserId == CurrentUser.Id)?.ConnectionIds;


            if (connectionIds != null)
            {
                connectionIds.Remove(connectionId);
            }

        }

        public bool IsOnline(int userId)
        {
            return userConnection.Any(uc => uc.UserId == userId && uc.ConnectionIds.Any());
        }

        private User GetCurrentLoggedUser()
        {
            try
            {
                var currentUserId = httpContextAccessor.HttpContext.User
                .FindFirst(c => c.Type == "Id").Value;
                
                return pustokDbContext.Users
                    .Include(u => u.UserRole)
                    .Single(u => u.Id == Convert.ToInt32(currentUserId));

            }
            catch (Exception ex)
            {
                return null;
            }


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
        public List<User> GetWholeClients()
        {
            var clients = pustokDbContext.Users.Where(user => !user.UserRole.Any()).ToList();

            return clients;
        }
    }
}
