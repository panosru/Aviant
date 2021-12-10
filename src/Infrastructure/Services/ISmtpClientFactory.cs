namespace Aviant.DDD.Infrastructure.Services;

using MailKit.Net.Smtp;

public interface ISmtpClientFactory
{
    public SmtpClient GetSmtpClient();
}
