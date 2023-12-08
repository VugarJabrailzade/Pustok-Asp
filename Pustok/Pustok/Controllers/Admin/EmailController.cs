using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NETCore.MailKit.Core;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Admin
{
    [Route("admin/email")]
    [Authorize(Roles ="admin")]
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly PustokDbContext _pustokDbContext;
        private readonly EmailConfiguration emailConfiguration;

        public EmailController(IEmailSender emailSender, PustokDbContext pustokDbContext)
        {
            _emailSender = emailSender;
            _pustokDbContext = pustokDbContext;
        }


        [HttpGet("list",Name ="email-list")]
        public IActionResult Emails()
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
            List<string> to = model.To;
            var title = model.Title;
            var content = model.Content;


            var message = new EmailMessage(to, title, content );
            _emailSender.SendEmail(message);

            _pustokDbContext.EmailMessageContents.Add(model);
            _pustokDbContext?.SaveChanges();
            

            return RedirectToAction("emails", "email");
        }


        [HttpDelete("delete", Name ="delete-email")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                var emails = await _pustokDbContext.EmailMessageContents.FirstOrDefaultAsync(x=> x.Id == id);
                if (emails == null) return NotFound();

                _pustokDbContext.EmailMessageContents.Remove(emails);
                await _pustokDbContext.SaveChangesAsync();

                var response = new
                {
                    deleteid = id,
                    title = emails.Title,
                };

                return Json(response);

                //return RedirectToRoute("email-list");
            }
            catch (Exception e)
            {
                throw e;
            }


        }


    }
}
