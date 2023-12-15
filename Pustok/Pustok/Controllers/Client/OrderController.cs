using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Hubs;
using Pustok.Services.Abstract;
using Pustok.Services.Concretes;
using Pustok.ViewModels;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Client
{
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly PustokDbContext _pustokDbContext;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IHubContext<AlertHub> _hubContext;

        public OrderController(PustokDbContext pustokDbContext, IUserService userService, IOrderService orderService, IHubContext<AlertHub> hubContext)
        {
            _pustokDbContext = pustokDbContext;
            _userService = userService;
            _orderService = orderService;
            _hubContext = hubContext;
        }


        [HttpGet("execute")]
        public async Task<IActionResult> Execute()
        {

            var order = new Order
            {
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.New,
                User = _userService.CurrentUser,
                TrackingCode = _orderService.GenerateTrackingCode()
            };


            var basketProduct = _pustokDbContext.BasketProducts.Where(x=> x.User == _userService.CurrentUser).ToList();




            if (basketProduct.Count == 0)
            {
                return RedirectToAction("cart", "basket");
            }
            
                order.OrderProducts = basketProduct
                .Select(bp => new OrderProduct
                {
                    ColorId = bp.ColorId,
                    SizeId = bp.SizeId,
                    ProductId = bp.ProductId,
                    Quantity = bp.Quantity,
                    Order = order
                })
                .ToList();

            _pustokDbContext.Orders.Add(order);

            _pustokDbContext.BasketProducts.RemoveRange(basketProduct);


            var notification = _orderService.CreatOrderNotifications(order);

            foreach(var notifications in notification)
            {
                var connectionIds =  _userService.GetUserConnection(notifications.UserId);

                foreach (var connectionId in connectionIds)
                {
                    var model = new NotificationViewModel
                    {
                        Title = notifications.Title,
                        Content = notifications.Content,
                        CreatedAt = notifications.CreatedDate.ToString("dd/MM/yyyy HH:mm")
                    };

                    await _hubContext.Clients
                        .Client(connectionId)
                        .SendAsync("Order", model);
                }

            }

      

            _pustokDbContext.SaveChanges();

            return RedirectToAction("Order","Dashboard");
            
                
            
        }
    }
}
