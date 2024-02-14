using FluentValidation;
using NotificationService.Core.Extensions;
using NotificationService.Core.Models;

namespace NotificationService.Application.Notification.Commands.SendSMS;

public class SendSMSCommandValidator : AbstractValidator<SendSMSCommand>
{
    public SendSMSCommandValidator()
    {
        RuleFor(c => c.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required");
        
        RuleFor(c => c.Content)
            .NotEmpty().WithMessage("Content is required")
            .Must(content => content.HaveValidLengthInKB(SMSNotificationMessage.MaxContentLength)).WithMessage($"Content must not exceed {SMSNotificationMessage.MaxContentLength}");
    }
}