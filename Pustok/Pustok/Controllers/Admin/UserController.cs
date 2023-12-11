    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.ViewModels.Auth;
using System.Linq;

namespace Pustok.Controllers.Admin
{
    [Route("admin/user")]
    [Authorize(Roles = RoleNames.SuperAdmin)]


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

            return View("Views/Admin/User/UserEdit.cshtml", model);
        }


        [HttpPost("{id}/edit")]
        public IActionResult Edit([FromRoute] int id, [FromForm] UserUpdateViewModel model)
        {
            var user = _pustokDbContext.Users.
                       Include(u => u.UserRole).
                       FirstOrDefault(n=> n.Id == id);

            if (user == null) return NoContent();
            
            user.Name = model.Name; 
            user.LastName = model.LastName;

            #region Role Management
            
            var roles = user.UserRole.Select(v=> v.Role); 


            // Remove Proces------

            var removableRole = roles.Where(x=> !model.SelectedRoles.Contains(x)).ToList();

            user.UserRole.RemoveAll(x => removableRole.Contains(x.Role));


            // Add Process ----

            var adorable = model.SelectedRoles.Where(r=> !roles.Contains(r)).ToList();

            var newRole = adorable.Select(r => new UserRole
            {
                Role = r,
                UserId = user.Id,
            });

            user.UserRole.AddRange(newRole);
            _pustokDbContext.SaveChanges();


            #endregion


            return RedirectToAction("index", "user");
        }



    }


}
