using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Hubs;
using Pustok.Services.Abstract;
using Pustok.Services.Concretes;
using Pustok.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Admin
{
    [Route("admin/broadcast")]
    public class BroadcastController : Controller
    {
        private readonly IHubContext<AlertHub> httpContext;
        private readonly IUserService userService;
        private readonly PustokDbContext _pustokcontext;

        public BroadcastController(IHubContext<AlertHub> httpContext, 
                                   IUserService userService, 
                                   PustokDbContext pustokcontext)
        {
            this.httpContext = httpContext;
            this.userService = userService;
            _pustokcontext = pustokcontext;
        }

       

        [HttpGet("list")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("send")]
        public IActionResult SendNotification()
        
        {
            return View("Views/Admin/Broadcast/Send.cshtml");
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification(BroadcastViewModel model)
        {
            var users = userService.GetWholeClients();
            List<Notifications> notifications = new List<Notifications>();

            var broadcast = new Notifications
            {
                Content = model.Content
            };

            await httpContext.Clients.All.SendAsync("ReceiveAllMessage", users, broadcast);

            _pustokcontext.Notifications.Add(broadcast);
            _pustokcontext.SaveChanges();


            return RedirectToAction("Views/Admin/Broadcast/Send.cshtml");


        }
    }
}
