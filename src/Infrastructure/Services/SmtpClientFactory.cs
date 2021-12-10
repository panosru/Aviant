namespace Aviant.DDD.Infrastructure.Services;

using MailKit.Net.Smtp;
using MailKit.Security;

public class SmtpClientFactory : ISmtpClientFactory
{
    private readonly string _address;

    private readonly string _password;

    private readonly int _port;

    private readonly string _username;

    private readonly bool _useSsl;

    public SmtpClientFactory(
        string address,
        int    port,
        bool   useSsl,
        string username,
        string password)
    {
        _address  = address;
        _port     = port;
        _useSsl   = useSsl;
        _username = username;
        _password = password;
    }

    #region ISmtpClientFactory Members

    public SmtpClient GetSmtpClient()
    {
        SmtpClient client = new();

        client.Connect(
            _address,
            _port,
            _useSsl
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.None);

        client.Authenticate(_username, _password);

        return client;
    }

    #endregion
}
