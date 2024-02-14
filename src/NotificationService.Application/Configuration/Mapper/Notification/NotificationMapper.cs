using AutoMapper;
using NotificationService.Application.Notification.Commands.SendEmail;
using NotificationService.Application.Notification.Commands.SendSMS;
using NotificationService.Application.Notification.Contracts.SendEmail;
using NotificationService.Application.Notification.Contracts.SendSMS;

namespace NotificationService.Application.Configuration.Mapper.Notification;

internal class NotificationMapper : Profile
{
    public NotificationMapper()
    {
        ViewToDomain();
    }
    
    private void ViewToDomain()
    {
        CreateMap<SendEmailRequestModel, SendEmailCommand>();
        CreateMap<SendSMSRequestModel, SendSMSCommand>();
    }
}