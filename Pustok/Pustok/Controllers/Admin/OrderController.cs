using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Hubs;
using Pustok.Services.Abstract;
using Pustok.Services.Concretes;
using Pustok.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Admin
{
    [Route("admin/orders")]
    [Authorize(Roles = RoleNames.SuperAdmin)]



    public class OrderController : Controller
    {
        private readonly PustokDbContext _pustokDbContext;
        private readonly IOrderService orderService;
        private readonly IUserService userService;
        private readonly IHubContext<AlertHub> hubContext;

        public OrderController(PustokDbContext pustokDbContext, 
                               IOrderService orderService, 
                               IUserService userService, 
                               IHubContext<AlertHub> hubContext)
        {
            _pustokDbContext = pustokDbContext;
            this.orderService = orderService;
            this.userService = userService;
            this.hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orders = _pustokDbContext.Orders.Include(x=> x.OrderProducts).ToList();
            if (orders == null) return NotFound();


            return View("Views/Admin/Orders/Index.cshtml",orders);
        }

        [HttpGet("{id}/edit")]
        public IActionResult Edit(int id)
        {
            var order = _pustokDbContext.Orders.Include(x => x.User).Include(x => x.OrderProducts).ThenInclude(x=> x.Product).
                FirstOrDefault(x => x.Id == id);

            if (order == null) return NotFound();

            return View("Views/Admin/Orders/OrderEdit.cshtml",order);
        }

        [HttpPost("{id}/edit",Name ="order-edit")]
        public async Task<IActionResult> Edit([FromRoute] int id, OrderStatus status)
        {
            var order = _pustokDbContext.Orders.Include(v=> v.User).FirstOrDefault(x=> x.Id == id);
            if (order == null) return NotFound();

            order.Status = status;

            var updatenotifications = orderService.UpdateOrderStatusNotifications(order, status);

            foreach(var notification in updatenotifications)
            {
                var connectionIds = userService.GetUserConnection(notification.UserId);

                foreach(var connectionId in connectionIds)
                {
                    var model = new NotificationViewModel
                    {
                        Title = notification.Title,
                        Content = notification.Content,
                        CreatedAt = notification.CreatedDate.ToString("dd/MM/yyyy HH:mm")
                    };

                    await hubContext.Clients
                       .Client(connectionId)
                       .SendAsync("UpdatedStatus", model);
                }
            }

            _pustokDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
