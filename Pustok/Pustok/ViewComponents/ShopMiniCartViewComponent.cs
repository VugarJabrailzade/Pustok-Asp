using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Database;
using Pustok.Services.Abstract;
using System.Linq;
using System.Threading.Tasks;
namespace Pustok.ViewComponents;

public class ShopMiniCartViewComponent : ViewComponent
{

    private readonly PustokDbContext _pustokDbContext;
    private readonly IUserService _userService;

    public ShopMiniCartViewComponent(PustokDbContext pustokDbContext, IUserService userService)
    {
        _pustokDbContext = pustokDbContext;
        _userService = userService;
    }


    public async Task<IViewComponentResult> InvokeAsync()
    {
        var basketProduct = _pustokDbContext.BasketProducts.Where(x=> x.User == _userService.CurrentUser).
            Include(x => x.Product).
            ToList();

        return View(basketProduct);
    }

}
