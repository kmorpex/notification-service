using NotificationService.Core.Models;
using NotificationService.Core.Providers;

namespace NotificationService.Infrastructure.Notification.Providers.SMS.Fake;

public class FakeSMSProvider : ISMSProvider
{
    public Task SendAsync(SMSNotificationMessage smsMessage, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"📲 => SMS succesfully sent to {smsMessage.PhoneNumber} at {DateTime.UtcNow:HH:mm:ss zzz}");
        return Task.CompletedTask;
    }
}