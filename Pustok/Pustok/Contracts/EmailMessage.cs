
using System.Collections.Generic;
using System.Linq;
using MailKit;
using MimeKit;
using Pustok.Database.Abstracts;

namespace Pustok.Contracts
{
    public class EmailMessage
    {
        public int EmailId { get; set; }
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x,x)));
            Subject = subject;
            Content = content;
        }
        public EmailMessage() { }
    }
}
