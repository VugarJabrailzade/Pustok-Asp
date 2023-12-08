using MimeKit;
using Pustok.Contracts;

namespace Pustok.Services.Abstract
{
    public interface IEmailSender
    {
        void SendEmail(EmailMessage message);
        MimeMessage CreateEmailMessage(EmailMessage message);
        void Send(MimeMessage mailMessage);
    }
}
