using MediatR;

namespace NotificationService.Core.Abstractions;

// MediaR notifications are used to implement the Publish-Subscribe pattern
public interface IDomainEvent : INotification
{
}