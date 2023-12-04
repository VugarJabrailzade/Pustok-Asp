using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using System.Linq;

namespace Pustok.Controllers.Admin
{
    [Route("admin/orders")]
    public class OrderController : Controller
    {
        private readonly PustokDbContext _pustokDbContext;

        public OrderController(PustokDbContext pustokDbContext)
        {
            _pustokDbContext = pustokDbContext;
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
        public IActionResult Edit([FromRoute] int id, OrderStatus status)
        {
            var order = _pustokDbContext.Orders.Include(v=> v.User).FirstOrDefault(x=> x.Id == id);
            if (order == null) return NotFound();

            order.Status = status;

            _pustokDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
