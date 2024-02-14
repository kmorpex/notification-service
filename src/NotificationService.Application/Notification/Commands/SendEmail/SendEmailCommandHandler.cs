using ErrorOr;
using NotificationService.Application.Abstractions.Messaging;
using NotificationService.Core.Models;
using NotificationService.Core.Providers;
using NotificationService.Core.ValueObjects;

namespace NotificationService.Application.Notification.Commands.SendEmail;

public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, ErrorOr<Success>>
{
    private readonly INotificationServiceFactory _notificationService;
    
    public SendEmailCommandHandler(INotificationServiceFactory notificationService)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }
    
    public async Task<ErrorOr<Success>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var emailMessage = EmailNotificationMessage.Create(
            to: request.To.Select(x => new EmailAddress(x)).ToList(),
            subject: request.Subject,
            content: request.Content
        );

        await _notificationService.SendEmailAsync(emailMessage, cancellationToken);

        return Result.Success;
    }
}