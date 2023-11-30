using Pustok.Database.DomainModels;

namespace Pustok.Services.Abstract
{
    public interface IBasketService
    {
        void AddOrIncrementQuantity(int productId, int sizeId, int colorId, User user);
    }
}
