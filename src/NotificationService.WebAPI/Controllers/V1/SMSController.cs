using Asp.Versioning;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Abstractions.Services;
using NotificationService.Application.Notification.Contracts.SendSMS;

namespace NotificationService.WebAPI.Controllers.V1;

[ApiVersion(1.0)]
public class SMSController(INotificationAppService notificationAppService) : ApiControllerAbstract
{
    private readonly INotificationAppService _notificationAppService = notificationAppService ?? throw new ArgumentNullException(nameof(notificationAppService));

    [HttpPost("send")]
    public async Task<ApiResponse> SendSMS([FromBody] SendSMSRequestModel request)
    {
        var result = await _notificationAppService.SendSMSAsync(request);

        return Response(result);
    }
}