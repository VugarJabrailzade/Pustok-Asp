using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Database;
using Pustok.ViewModels.Auth;
using System.Linq;

namespace Pustok.Controllers.Admin
{
    [Route("admin/user")]
    [Authorize(Roles ="admin")]
    public class UserController : Controller
    {
        private readonly PustokDbContext _pustokDbContext;

        public UserController(PustokDbContext pustokDbContext)
        {
            _pustokDbContext = pustokDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        
        
        {
            var user = _pustokDbContext.Users
                .Include(u=> u.UserRole)
                .OrderBy(n=> n.Name).ToList();

            return View("Views/Admin/User/Users.cshtml", user);
        }

        [HttpGet("{id}/edit")]
        public IActionResult Edit(int id)
        {
            var user = _pustokDbContext.Users.
                Include(u => u.UserRole)
                .ThenInclude(u => u.Role)
                .FirstOrDefault(n => n.Id == id);

            if (user == null) return NoContent();

            var model = new UserUpdateViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                SelectedRoles = user.UserRole.Select(x => x.Role).ToArray()

            };

            return View("Views/Admin/User/UserEdit.cshtml");
        }
    }
}
