using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using System.Linq;

namespace Pustok.Services.Concretes;

public class BasketService : IBasketService
{
    private readonly PustokDbContext pustokDbContext;
    private readonly IUserService userService;

    public BasketService(PustokDbContext pustokDbContext)
    {
        this.pustokDbContext = pustokDbContext;
    }


    public void AddOrIncrementQuantity(int productId, int sizeId, int colorId, User user)
    {

        var basket = pustokDbContext.BasketProducts.FirstOrDefault(pr => pr.ProductId == productId && 
                                                                         pr.SizeId == sizeId && 
                                                                         pr.ColorId == colorId && 
                                                                         pr.UserId == user.Id);


        if(basket != null)
        {
            basket.Quantity++;
            return;
        }

        var basketProduct = new BasketProduct
        {
            ProductId = productId,
            SizeId = sizeId,
            ColorId = colorId,
            User = user,
            Quantity = 1
        };

        pustokDbContext.BasketProducts.Add(basketProduct);
    }
}


