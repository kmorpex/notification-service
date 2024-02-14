using NotificationService.Core.Models;

namespace NotificationService.Core.Providers;

public interface IEmailProvider
{
    Task SendAsync(EmailNotificationMessage emailMessage, CancellationToken cancellationToken = default);
}