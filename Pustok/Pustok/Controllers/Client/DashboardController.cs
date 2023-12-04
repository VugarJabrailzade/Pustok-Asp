using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Database;
using Pustok.Services.Abstract;
using Pustok.ViewModels.Dashboard;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Client
{
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        private readonly PustokDbContext _pustokDbContext;
        private readonly IUserService _userService;

        public DashboardController(PustokDbContext pustokDbContext, IUserService userService)
        {
            _pustokDbContext = pustokDbContext;
            _userService = userService;
        }

        [HttpGet("order")]
        public IActionResult Order()
        {
            var order = _pustokDbContext.Orders.Include(x => x.OrderProducts).
                        ThenInclude(v => v.Product).Where(x => x.User == _userService.CurrentUser).ToList();


            return View(order);
        }

        [HttpGet("order-details", Name="order-details-modal")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var orderProducts  = await _pustokDbContext.OrdersProducts
           .Where(op => op.OrderId == id && op.Order.User == _userService.CurrentUser)
           .Include(op => op.Product)
           .Include(op => op.Product.Category)
           .Include(op => op.Size)
           .Include(op => op.Color)
           .ToListAsync();

            if(orderProducts.Count == null) return NotFound();

            return View(orderProducts);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
