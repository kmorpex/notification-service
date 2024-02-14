using NotificationService.Infrastructure.Background.Configuration;
using NotificationService.Infrastructure.Background.Shared;

namespace NotificationService.Infrastructure.Background;

public interface IBackgroundTaskQueue
{
    void QueueBackgroundWorkItem(int priority, BackgroundWorkItemDelegate workItem);
    Task<PriorityBackgroundTask> DequeueAsync(CancellationToken cancellationToken);
}
