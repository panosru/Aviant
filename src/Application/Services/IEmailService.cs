namespace Aviant.DDD.Application.Services
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        IEmailService From(string name, string address);

        IEmailService FromServer();

        IEmailService To(string name, string address);

        IEmailService ToServer();

        IEmailService WithSubject(string subject);
        
        IEmailService Message();

        IEmailService WithBodyHtml(string body);

        IEmailService WithBodyPlain(string body);
        
        bool Send();

        Task<bool> SendAsync();
    }
}