using NotificationService.Core.Models;
using NotificationService.Core.Providers;

namespace NotificationService.Infrastructure.Notification.Providers.Email.Fake;

public class FakeEmailProvider : IEmailProvider
{
    public async Task SendAsync(EmailNotificationMessage emailMessage, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"📧 => Email succesfully sent {string.Join(", ", emailMessage.To)} at {DateTime.UtcNow:HH:mm:ss zzz}");
    }
}