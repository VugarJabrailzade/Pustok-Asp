using MimeKit;
using Pustok.Database.DomainModels;
using System.Net.Mail;

namespace Pustok.Services.Concretes;

public class EmailSender
{
    private readonly EmailConfiguration _emailConfig;

    public EmailSender(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public void SendEmail(EmailMessage message)
    {
        var emailMessage = (message);

        SendEmail(emailMessage);
    }

    //private MimeMessage CreateEmailMessage(EmailMessage message)
    //{
    //    var emailMessage = new MimeMessage();
    //    emailMessage.From.Add(new MailboxAddress(_emailConfig));
    //    emailMessage.To.AddRange(message.To);
    //    emailMessage.Subject= message.Subject;
    //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

    //    return emailMessage;
    //}

    //private void Send(MimeMessage mailMessage)
    //{
    //    using(var client = new SmtpClient)
    //    {
    //        try
    //        {
    //            client.
    //        }



    //    }
    //}
}
