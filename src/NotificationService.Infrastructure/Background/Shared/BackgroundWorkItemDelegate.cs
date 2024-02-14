namespace NotificationService.Infrastructure.Background.Shared;

public delegate Task BackgroundWorkItemDelegate(CancellationToken cancellationToken = default);