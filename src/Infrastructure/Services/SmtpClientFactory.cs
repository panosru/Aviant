namespace Aviant.DDD.Infrastructure.Services
{
    using Application.Services;
    using MailKit.Net.Smtp;
    using MailKit.Security;

    public class SmtpClientFactory : ISmtpClientFactory
    {
        private readonly string _address;

        private readonly int _port;

        private readonly bool _useSsl;

        private readonly string _username;

        private readonly string _password;

        public SmtpClientFactory(string address, int port, bool useSsl,
            string username,
            string password)
        {
            _address = address;
            _port = port;
            _useSsl = useSsl;
            _username = username;
            _password = password;
        }


        public SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            
            client.Connect(
                _address,
                _port,
                _useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);
            
            client.Authenticate(_username, _password);

            return client;
        }
    }
}