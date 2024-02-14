namespace NotificationService.Application.Notification.Contracts.SendEmail;

public class SendEmailRequestModel
{
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}