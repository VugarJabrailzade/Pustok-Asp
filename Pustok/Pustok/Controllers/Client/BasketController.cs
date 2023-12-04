using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using Pustok.ViewModels.Basket;
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
                    userService.CurrentUser);

        pustokDbContext.SaveChanges();

        return RedirectToAction("index", "home");
    }

    [HttpGet("cart")]
    public IActionResult GetBasketProduct()
    {
        var basketProduct = pustokDbContext.BasketProducts.Where(x=> x.User.Id == userService.CurrentUser.Id).
            Include(bp=> bp.Product).
            Include(bp=> bp.Color).
            Include(bp=> bp.Size).ToList();

        return View(basketProduct);
    }

    [HttpGet("remove-basket-product")]
    public IActionResult RemoveBasket( int basketProductId)
    {
        var basketProduct = pustokDbContext.BasketProducts.FirstOrDefault(x => x.UserId == userService.CurrentUser.Id &&
                            x.Id == basketProductId);

        if(basketProduct == null) NotFound();


        pustokDbContext.BasketProducts.Remove(basketProduct);
        pustokDbContext.SaveChanges();

        return RedirectToAction("cart");


    }

    [HttpGet]
    public IActionResult IncreaseBasketProduct(int basketProductId) 
    {
        var basketProduct = pustokDbContext.BasketProducts.FirstOrDefault(x => x.UserId == userService.CurrentUser.Id &&
                            x.Id == basketProductId);

        if (basketProduct == null) NotFound();

        basketProduct.Quantity++;


        pustokDbContext.SaveChanges();

        return Json(new BasketQuantityUpdateResponseViewModel
        {
            Total = basketProduct.Quantity * basketProduct.Product.Price,
            Quantity = basketProduct.Quantity
        });


    }
    [HttpGet]
    public IActionResult DecreaseBasketProduct(int basketProductId)
    {
        var basketProduct = pustokDbContext.BasketProducts.FirstOrDefault(x => x.UserId == userService.CurrentUser.Id &&
                            x.Id == basketProductId);

        if (basketProduct == null) NotFound();

        basketProduct.Quantity--;

        IActionResult actionResult;
        if(basketProduct.Quantity == 0)
        {
            pustokDbContext.BasketProducts.Remove(basketProduct);
            actionResult = NoContent();

        }
        else
        {
            var updateBasketResponse = new BasketQuantityUpdateResponseViewModel
            {
                Total = basketProduct.Quantity * basketProduct.Product.Price,
                Quantity = basketProduct.Quantity
            };

            actionResult = Json(updateBasketResponse);


        }
            pustokDbContext.SaveChanges();
            return actionResult;

    }



    public IActionResult Index()
    {
        return View();
    }


}
