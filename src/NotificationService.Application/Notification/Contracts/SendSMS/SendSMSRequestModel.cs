namespace NotificationService.Application.Notification.Contracts.SendSMS;

public class SendSMSRequestModel
{
    public string PhoneNumber { get; set; }
    public string Content { get; set; }
}