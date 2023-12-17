namespace Pustok.ViewModels.Basket;

public class BasketCookieViewModel
{
    public BasketCookieViewModel(int productId, int colorId, int sizeId, int quantity = 1)
    {
        ProductId = productId;
        ColorId = colorId;
        SizeId = sizeId;
        Quantity = quantity;
    }

    public int  Quantity { get; set; }
    public int ProductId { get; set; }
    public int  ColorId { get; set; }
    public int SizeId { get; set; }
}
