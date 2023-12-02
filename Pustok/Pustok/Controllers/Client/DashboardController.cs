using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Database;
using Pustok.Services.Abstract;
using System.Linq;

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
                        ThenInclude(v => v.Product).Where(x => x.User == _userService.GetCurrentLoggedUser()).ToList();


            return View(order);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
