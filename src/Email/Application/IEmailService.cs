namespace Aviant.Application.Email;

public interface IEmailService
{
    public IEmailService From(string name, string address);

    public IEmailService To(string name, string address);

    public IEmailService WithSubject(string subject);

    public IEmailService WithBodyHtml(string body);

    public IEmailService WithBodyPlain(string body);

    public IEmailService AttachFile(string filePath);

    public bool Send();

    public Task<bool> SendAsync(CancellationToken cancellationToken = default);
    
    public IEmailService Message();
}
