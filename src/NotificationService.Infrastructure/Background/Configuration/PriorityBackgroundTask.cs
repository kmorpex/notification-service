using NotificationService.Infrastructure.Background.Shared;

namespace NotificationService.Infrastructure.Background.Configuration;

public class PriorityBackgroundTask
{
    public BackgroundWorkItemDelegate TaskFunction { get; set; }
    public int Priority { get; set; }
}