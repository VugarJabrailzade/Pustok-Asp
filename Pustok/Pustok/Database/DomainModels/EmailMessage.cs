using MimeKit;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Pustok.Database.DomainModels
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        //public EmailMessage(IEnumerable<string> to, string subject, string content)
        //{
        //    To = new List<MailboxAddress>();
        //    To.AddRange(to.Select(x=> new MailboxAddress(x)).ToList());
        //    Subject = subject;
        //    Content = content;
        //}
    }
}
