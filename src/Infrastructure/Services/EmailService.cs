namespace Aviant.DDD.Infrastructure.Services;

using Application.Services;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

public class EmailService : IEmailService, IDisposable
{
    private readonly string _receiverEmail;

    private readonly string _receiverName;

    private readonly string _senderEmail;

    private readonly string _senderName;

    private readonly ISmtpClientFactory _smtpClientFactory;

    private MimeMessage _message;

    private SmtpClient _smtpClient;

    public EmailService(
        string             senderName,
        string             senderEmail,
        string             receiverName,
        string             receiverEmail,
        ISmtpClientFactory smtpClientFactory,
        MimeMessage        message,
        SmtpClient         smtpClient)
    {
        _senderName        = senderName;
        _senderEmail       = senderEmail;
        _receiverName      = receiverName;
        _receiverEmail     = receiverEmail;
        _smtpClientFactory = smtpClientFactory;
        _message           = message;
        _smtpClient        = smtpClient;
    }

    #region IEmailService Members

    public IEmailService From(string name, string address)
    {
        _message.From.Add(new MailboxAddress(name, address));

        return this;
    }

    public IEmailService FromServer()
    {
        _message.From.Add(new MailboxAddress(_senderName, _senderEmail));

        return this;
    }

    public IEmailService To(string name, string address)
    {
        _message.To.Add(new MailboxAddress(name, address));

        return this;
    }

    public IEmailService ToServer()
    {
        _message.To.Add(new MailboxAddress(_receiverName, _receiverEmail));

        return this;
    }

    public IEmailService WithSubject(string subject)
    {
        _message.Subject = subject;

        return this;
    }

    public IEmailService Message()
    {
        _smtpClient = _smtpClientFactory.GetSmtpClient();
        _message    = new MimeMessage();

        return this;
    }

    public IEmailService WithBodyHtml(string body)
    {
        _message.Body = new TextPart(TextFormat.Html)
        {
            Text = body
        };

        return this;
    }

    public IEmailService WithBodyPlain(string body)
    {
        _message.Body = new TextPart("plain")
        {
            Text = body
        };

        return this;
    }

    public bool Send()
    {
        _smtpClient.Send(_message);
        _smtpClient.Disconnect(true);
        _smtpClient.Dispose();

        return true;
    }

    public Task<bool> SendAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Send());

    #endregion

    /// <inheritdoc />
    public void Dispose()
    {
        _message.Dispose();
        _smtpClient.Dispose();
    }
}
