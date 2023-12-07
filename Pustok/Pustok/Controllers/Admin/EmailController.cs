using Microsoft.AspNetCore.Mvc;

namespace Pustok.Controllers.Admin
{
    [Route("admin/email")]
    public class EmailController : Controller
    {
        [HttpGet]
        public IActionResult Email()
        {
            return View();
        }
    }
}
