namespace Aviant.DDD.Application.Services
{
    using MailKit.Net.Smtp;

    public interface ISmtpClientFactory
    {
        SmtpClient GetSmtpClient();
    }
}