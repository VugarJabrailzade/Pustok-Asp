using MailKit.Net.Smtp;
using MimeKit;
using Pustok.Contracts;
using Pustok.Services.Abstract;

namespace Pustok.Services.Concretes;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfig;

    public EmailSender(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public void SendEmail(EmailMessage message)
    {
        var emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }

    private MimeMessage CreateEmailMessage(EmailMessage message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfig.Username,_emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

        return emailMessage;
    }

    MimeMessage IEmailSender.CreateEmailMessage(EmailMessage message)
    {
        throw new System.NotImplementedException();
    }

    private void Send(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.Authenticate(_emailConfig.Username, _emailConfig.Password, default);

                client.Send(mailMessage);

            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
            }

        }
    }

    void IEmailSender.Send(MimeMessage mailMessage)
    {
        throw new System.NotImplementedException();
    }
}
