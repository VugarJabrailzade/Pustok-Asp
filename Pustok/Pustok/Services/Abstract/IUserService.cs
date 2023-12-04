using Pustok.Database.DomainModels;

namespace Pustok.Services.Abstract
{
    public interface IUserService
    {
        User CurrentUser { get; }
        bool IsAuthenticated { get; }
        string GetFullName(User user);
    }
}
