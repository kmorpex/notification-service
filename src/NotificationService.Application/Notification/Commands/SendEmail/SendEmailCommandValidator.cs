using FluentValidation;
using NotificationService.Core.Extensions;
using NotificationService.Core.Models;

namespace NotificationService.Application.Notification.Commands.SendEmail;

public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
{
    public SendEmailCommandValidator()
    {
        RuleFor(x => x.To)
            .NotEmpty().WithMessage("At least one recipient is required")
            .Must(emails => emails.Count <= EmailNotificationMessage.MaxRecipients).WithMessage($"Maximum {EmailNotificationMessage.MaxRecipients} recipients are allowed")
            .ForEach(email => email.EmailAddress()).WithMessage("Invalid email address");

        RuleFor(c => c.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(EmailNotificationMessage.MaxSubjectLength).WithMessage($"Subject must not exceed {EmailNotificationMessage.MaxSubjectLength} characters");
        
        RuleFor(c => c.Content)
            .NotEmpty().WithMessage("Content is required")
            .Must(content => content.HaveValidLengthInKB(EmailNotificationMessage.MaxContentLengthInKB)).WithMessage($"Content must not exceed {EmailNotificationMessage.MaxContentLengthInKB}KB");
    }
}