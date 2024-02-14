using NotificationService.Core.ValueObjects;

namespace NotificationService.Core.Models;

public class SMSNotificationMessage
{
    public PhoneNumber PhoneNumber { get; }
    public string Content { get; }
    
    public const int MaxContentLength = 160;
    
    private SMSNotificationMessage()
    {
    }
    
    private SMSNotificationMessage(PhoneNumber phoneNumber, string content)
    {
        PhoneNumber = phoneNumber;
        Content = content;
    }
    
    public static SMSNotificationMessage Create(PhoneNumber phoneNumber, string content)
    {
        ValidatePhoneNumber(phoneNumber);
        ValidateContent(content);
        
        return new SMSNotificationMessage(phoneNumber, content);
    }
    
    private static void ValidatePhoneNumber(PhoneNumber phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number is required");
        }
    }
    
    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required");
        }
        
        if (content.Length > MaxContentLength)
        {
            throw new ArgumentException($"Content must not exceed {MaxContentLength} characters");
        }
    }
}