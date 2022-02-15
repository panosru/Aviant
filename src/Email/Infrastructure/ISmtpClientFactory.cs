namespace Aviant.Infrastructure.Email;

using MailKit.Net.Smtp;

public interface ISmtpClientFactory
{
    public SmtpClient GetSmtpClient();
}
