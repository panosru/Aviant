using Aviant.Application.Email;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Aviant.Infrastructure.Email;

public class EmailService : IEmailService, IDisposable
{
    private readonly ISmtpClientFactory _smtpClientFactory;
    private SmtpClient _smtpClient;
    private MimeMessage _message;

    private readonly string _globalFromName;
    private readonly string _globalFromEmail;

    private bool _disposed; // To detect redundant calls

    public EmailService(
        ISmtpClientFactory smtpClientFactory,
        string globalFromName,
        string globalFromEmail)
    {
        _smtpClientFactory = smtpClientFactory;
        _globalFromName    = globalFromName;
        _globalFromEmail   = globalFromEmail;
        
        Message();
    }

    #region IEmailService Members

    public IEmailService From(string name, string address)
    {
        _message.From.Clear();
        _message.From.Add(new MailboxAddress(name, address));

        return this;
    }
    
    public IEmailService To(string name, string address)
    {
        _message.To.Clear();
        _message.To.Add(new MailboxAddress(name, address));

        return this;
    }

    public IEmailService WithSubject(string subject)
    {
        _message.Subject = subject;

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
    
    public IEmailService AttachFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        var attachment = new MimePart("application", "octet-stream")
        {
            Content = new MimeContent(File.OpenRead(filePath), ContentEncoding.Default),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = Path.GetFileName(filePath)
        };

        // Ensure the message has a multipart/mixed container for the attachments
        if (!(_message.Body is Multipart multipart))
        {
            multipart = new Multipart("mixed")
            {
                _message.Body, // Add the existing body
                attachment     // Add the new attachment
            };

            _message.Body = multipart;
        }
        else
        {
            multipart.Add(attachment);
        }

        return this;
    }

    public bool Send()
    {
        _smtpClient.Send(_message);
        Message(); // Reset for next message

        return true;
    }

    public async Task<bool> SendAsync(CancellationToken cancellationToken = default)
    {
        await _smtpClient.SendAsync(_message, cancellationToken).ConfigureAwait(false);
        Message(); // Reset for next message

        return true;
    }

    public IEmailService Message()
    {
        _smtpClient = _smtpClientFactory.GetSmtpClient();
        _message = new MimeMessage();
        // Automatically use the global sender as the default
        _message.From.Add(new MailboxAddress(_globalFromName, _globalFromEmail));

        return this;
    }

    #endregion

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Prevent finalizer from running.
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return; // If already disposed, then return.

        if (disposing)
        {
            // Free any managed objects here.
            _message.Dispose();
            if (_smtpClient.IsConnected)
            {
                _smtpClient.Disconnect(true);
            }
            _smtpClient.Dispose();
        }
        
        _disposed = true; // Mark as disposed.
    }

    ~EmailService()
    {
        Dispose(false);
    }
}
