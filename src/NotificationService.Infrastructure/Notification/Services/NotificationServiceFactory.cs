using NotificationService.Core.Models;
using NotificationService.Core.Providers;

namespace NotificationService.Infrastructure.Notification.Services;

public class NotificationServiceFactory : INotificationServiceFactory
{
    private readonly IEmailProvider _emailProvider;
    private readonly ISMSProvider _smsProvider;

    public NotificationServiceFactory(
        IEmailProvider emailProvider,
        ISMSProvider smsProvider)
    {
        _emailProvider = emailProvider ?? throw new ArgumentNullException(nameof(emailProvider));
        _smsProvider = smsProvider ?? throw new ArgumentNullException(nameof(smsProvider));
    }

    public Task SendEmailAsync(EmailNotificationMessage emailMessage, CancellationToken cancellationToken = default)
    {
        return _emailProvider.SendAsync(emailMessage, cancellationToken);
    }

    public Task SendSMSAsync(SMSNotificationMessage smsMessage, CancellationToken cancellationToken = default)
    {
        return _smsProvider.SendAsync(smsMessage, cancellationToken);
    }
}