using Microsoft.AspNetCore.Mvc;
using Pustok.Database;
using Pustok.Services.Abstract;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.ViewComponents;

public class NotificationViewComponent : ViewComponent
{

    private readonly PustokDbContext _pustokDbContext;
    private readonly IUserService _userService;

    public NotificationViewComponent(PustokDbContext pustokDbContext, IUserService userService)
    {
        _pustokDbContext = pustokDbContext;
        _userService = userService;
    }


    public async Task<IViewComponentResult> InvokeAsync()
    {
        var notifications = _pustokDbContext.Notifications.Where(x => x.User == _userService.CurrentUser).
                            OrderByDescending(n=> n.CreatedDate).
                            ToList();

        return View(notifications);
    }
}
