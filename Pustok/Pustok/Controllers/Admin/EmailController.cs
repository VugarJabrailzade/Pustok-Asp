using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NETCore.MailKit.Core;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Pustok.Controllers.Admin
{
    [Route("admin/email")]
    [Authorize(Roles ="admin")]
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly PustokDbContext _pustokDbContext;

        public EmailController(IEmailSender emailSender, PustokDbContext pustokDbContext)
        {
            _emailSender = emailSender;
            _pustokDbContext = pustokDbContext;
        }


        [HttpGet("list")]
        public IActionResult Email()
        {
            var emails = _pustokDbContext.EmailMessageContents.ToList();

            return View("Views/Admin/Email/Email.cshtml", emails);
        }

        [HttpGet("emailSend", Name ="email-send")]
        public IActionResult EmailSend()
        {
            return View("Views/Admin/Email/EmailSend.cshtml");
        }

        [HttpPost("list",Name ="send-email")]
        public IActionResult EmailSend(EmailMessageContent model)
        {
            var to = new List<string>(model.To).ToString();
            var title = model.Title;
            var content = model.Content;

            var message = new EmailMessage(new string[] { to }, title, content );
            _emailSender.SendEmail(message);

            _pustokDbContext.EmailMessageContents.Add(model);
            _pustokDbContext?.SaveChanges();
            

            return View("Views/Admin/Email/Email.cshtml");
        }
    }
}
