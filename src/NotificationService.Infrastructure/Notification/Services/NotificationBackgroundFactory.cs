using NotificationService.Core.Models;
using NotificationService.Core.Providers;
using NotificationService.Infrastructure.Background;

namespace NotificationService.Infrastructure.Notification.Services;

public class BackgroundNotificationServiceFactory : INotificationServiceFactory
{
    private readonly INotificationServiceFactory _notificationServiceFactory;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public BackgroundNotificationServiceFactory(
        INotificationServiceFactory notificationServiceFactory, 
        IBackgroundTaskQueue backgroundTaskQueue)
    {
        _notificationServiceFactory = notificationServiceFactory ?? throw new ArgumentNullException(nameof(notificationServiceFactory));
        _backgroundTaskQueue = backgroundTaskQueue ?? throw new ArgumentNullException(nameof(backgroundTaskQueue));
    }

    public Task SendEmailAsync(EmailNotificationMessage emailMessage, CancellationToken cancellationToken = default)
    {
        var _localEmailMessage = emailMessage;
        var _localCancellationToken = cancellationToken;
        
        _backgroundTaskQueue.QueueBackgroundWorkItem((int)PriorityLevel.Medium, async _ =>
        {
            await _notificationServiceFactory.SendEmailAsync(_localEmailMessage, _localCancellationToken);
        });
        
        return Task.CompletedTask;
    }

    public Task SendSMSAsync(SMSNotificationMessage smsMessage, CancellationToken cancellationToken = default)
    {
        var localSMSMessage = smsMessage;
        var localCancellationToken = cancellationToken;
        
        _backgroundTaskQueue.QueueBackgroundWorkItem((int)PriorityLevel.Medium, async token =>
        {
            await _notificationServiceFactory.SendSMSAsync(localSMSMessage, localCancellationToken);
        });
        
        return Task.CompletedTask;
    }
}