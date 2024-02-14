using ErrorOr;
using NotificationService.Application.Abstractions.Messaging;

namespace NotificationService.Application.Notification.Commands.SendEmail;

public sealed record SendEmailCommand(List<string> To, string Subject, string Content) : ICommand<ErrorOr<Success>>;