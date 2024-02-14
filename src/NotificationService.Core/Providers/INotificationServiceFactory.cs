using NotificationService.Core.Models;

namespace NotificationService.Core.Providers;

public interface INotificationServiceFactory
{
    Task SendEmailAsync(EmailNotificationMessage emailMessage, CancellationToken cancellationToken = default);
    Task SendSMSAsync(SMSNotificationMessage smsMessage, CancellationToken cancellationToken = default);
}