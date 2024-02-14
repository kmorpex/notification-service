using ErrorOr;
using NotificationService.Application.Notification.Contracts.SendEmail;
using NotificationService.Application.Notification.Contracts.SendSMS;

namespace NotificationService.Application.Abstractions.Services;

public interface INotificationAppService
{
    Task<ErrorOr<Success>> SendEmailAsync(SendEmailRequestModel model, CancellationToken ct = default);
    Task<ErrorOr<Success>> SendSMSAsync(SendSMSRequestModel model, CancellationToken ct = default);
}