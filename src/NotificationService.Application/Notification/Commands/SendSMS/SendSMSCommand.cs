using ErrorOr;
using NotificationService.Application.Abstractions.Messaging;

namespace NotificationService.Application.Notification.Commands.SendSMS;

public sealed record SendSMSCommand(string PhoneNumber, string Content) : ICommand<ErrorOr<Success>>;