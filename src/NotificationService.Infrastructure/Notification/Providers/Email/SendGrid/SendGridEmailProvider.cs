using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Options;
using NotificationService.Core.Models;
using NotificationService.Core.Providers;
using NotificationService.Infrastructure.Exceptions;
using SendGrid;
using SendGrid.Helpers.Mail;
using EmailAddress = SendGrid.Helpers.Mail.EmailAddress;

namespace NotificationService.Infrastructure.Notification.Providers.Email.SendGrid;

public class SendGridEmailProvider : IEmailProvider
{
    private readonly SendGridClient _sendGridClient;
    
    public SendGridEmailProvider(SendGridOptions? sendGridOptions)
    {
        if (string.IsNullOrWhiteSpace(sendGridOptions?.ApiKey))
        {
            throw new ArgumentException("SendGrid API key is required");
        }
        
        _sendGridClient = new SendGridClient(sendGridOptions.ApiKey);
    }

    public async Task SendAsync(EmailNotificationMessage emailMessage, CancellationToken cancellationToken = default)
    {
        var message = new SendGridMessage
        {
            From = new EmailAddress("test@test.ru"),
            Subject = emailMessage.Subject,
        };

        if (emailMessage.IsHtml)
            message.HtmlContent = emailMessage.Content;
        else
            message.PlainTextContent = emailMessage.Content;
        
        message.AddTos(emailMessage.To.Select(x => new EmailAddress(x.Value)).ToList());
        // message.AddBccs(emailMessage.Bcc.Select(x => new EmailAddress(x.Value)).ToList());
        // message.AddCcs(emailMessage.Cc.Select(x => new EmailAddress(x.Value)).ToList());

        try
        {
            var response = await _sendGridClient.SendEmailAsync(message, cancellationToken);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                string errorMessage = $"Failed to send email: {response.StatusCode}";
            
                switch (response.StatusCode)
                {
                    case HttpStatusCode.RequestTimeout:
                    case HttpStatusCode.TooManyRequests:
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.GatewayTimeout:
                        throw new RetryableException(errorMessage);
                    default:
                        throw new NonRetryableException(errorMessage);
                }
            }
        } 
        catch (HttpRequestException ex) when (ex.StatusCode == null)
        {
            if (ex.InnerException is SocketException socketEx)
            {
                switch (socketEx.SocketErrorCode)
                {
                    case SocketError.ConnectionRefused:
                    case SocketError.HostDown:
                    case SocketError.HostUnreachable:
                    case SocketError.NetworkDown:
                    case SocketError.NetworkUnreachable:
                    case SocketError.TimedOut:
                    case SocketError.HostNotFound:
                        throw new RetryableException($"Network-related error occurred: {socketEx.SocketErrorCode}", ex);
                    default:
                        throw;
                }
            }
        }
    }
}

public class SendGridOptions
{
    public string? ApiKey { get; init; }
}