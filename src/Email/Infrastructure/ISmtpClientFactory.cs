using MailKit.Net.Smtp;

namespace Aviant.Infrastructure.Email;

public interface ISmtpClientFactory
{
    public SmtpClient GetSmtpClient();
}
