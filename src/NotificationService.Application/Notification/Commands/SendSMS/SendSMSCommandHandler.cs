using ErrorOr;
using NotificationService.Application.Abstractions.Messaging;
using NotificationService.Core.Models;
using NotificationService.Core.Providers;
using NotificationService.Core.ValueObjects;

namespace NotificationService.Application.Notification.Commands.SendSMS;

public class SendSMSCommandHandler : ICommandHandler<SendSMSCommand, ErrorOr<Success>>
{
    private readonly INotificationServiceFactory _notificationService;
    
    public SendSMSCommandHandler(INotificationServiceFactory notificationService)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }
    
    public async Task<ErrorOr<Success>> Handle(SendSMSCommand request, CancellationToken cancellationToken)
    {
        var smsMessage = SMSNotificationMessage.Create(
            phoneNumber: new PhoneNumber(request.PhoneNumber),
            content: request.Content
        );

        await _notificationService.SendSMSAsync(smsMessage, cancellationToken);
        
        return Result.Success;
    }
}