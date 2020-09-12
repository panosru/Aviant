namespace Aviant.DDD.Infrastructure.Services
{
    #region

    using MailKit.Net.Smtp;

    #endregion

    public interface ISmtpClientFactory
    {
        SmtpClient GetSmtpClient();
    }
}