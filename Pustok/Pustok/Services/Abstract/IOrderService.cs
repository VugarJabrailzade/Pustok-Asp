using Pustok.Contracts;
using Pustok.Database.DomainModels;
using System.Collections.Generic;

namespace Pustok.Services.Abstract
{
    public interface  IOrderService
    {
        string GenerateTrackingCode();
        bool DoesCodeExist(string code);
        List<Notifications> CreatOrderNotifications(Order order);
        List<UserNotifications> UpdateOrderStatusNotifications(Order order, OrderStatus status);
    }
}
