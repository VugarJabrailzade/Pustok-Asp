using Pustok.Database.DomainModels;
using System.Collections.Generic;

namespace Pustok.Services.Abstract
{
    public interface IUserService
    {
        User CurrentUser { get; }
        bool IsAuthenticated { get; }
        bool IsCurrentUserInRole(params string[] roles);
        string GetFullName(User user);
        string GetCurrentUserFullName();
        bool IsUserSeeded(User user);
        List<User> GetWholeStaff();
    }
}
