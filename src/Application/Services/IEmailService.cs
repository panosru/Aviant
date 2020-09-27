namespace Aviant.DDD.Application.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        public IEmailService From(string name, string address);

        public IEmailService FromServer();

        public IEmailService To(string name, string address);

        public IEmailService ToServer();

        public IEmailService WithSubject(string subject);

        public IEmailService Message();

        public IEmailService WithBodyHtml(string body);

        public IEmailService WithBodyPlain(string body);

        public bool Send();

        public Task<bool> SendAsync(CancellationToken cancellationToken = default);
    }
}