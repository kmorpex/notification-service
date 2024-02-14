using NotificationService.Core.Models;

namespace NotificationService.Core.Providers;

public interface ISMSProvider
{
    Task SendAsync(SMSNotificationMessage smsMessage, CancellationToken cancellationToken = default);
}