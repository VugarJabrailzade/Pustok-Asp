using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Database;
using Pustok.Services.Abstract;
using System.Linq;

namespace Pustok.Controllers.Client;

[Route("basket")]
public class BasketController : Controller
{

    private readonly PustokDbContext pustokDbContext;
    private readonly IUserService userService;
    private readonly IBasketService basketService;
    private readonly IProductService productService;

    public BasketController(PustokDbContext pustokDbContext,
                            IUserService userService,
                            IBasketService basketService,
                            IProductService productService)
    {
        this.pustokDbContext = pustokDbContext;
        this.userService = userService;
        this.basketService = basketService;
        this.productService = productService;
    }

    [HttpGet("add-product")]
    public IActionResult AddProduct(int productId, int? sizeId, int? colorId)
    {

        basketService.AddOrIncrementQuantity(productId,
                    sizeId ?? productService.GetDefaultSizeId(productId),
                    colorId ?? productService.GetDefaultColorId(productId),
                    userService.GetCurrentLoggedUser());

        pustokDbContext.SaveChanges();

        return RedirectToAction("home", "index");
    }

    [HttpGet("cart")]
    public IActionResult GetBasketProduct()
    {
        var basketProduct = pustokDbContext.BasketProducts.Where(x=> x.User.Id == userService.GetCurrentLoggedUser().Id).
            Include(bp=> bp.Product).
            Include(bp=> bp.Color).
            Include(bp=> bp.Size).ToList();

        return View(basketProduct);
    }


    public IActionResult Index()
    {
        return View();
    }


}
