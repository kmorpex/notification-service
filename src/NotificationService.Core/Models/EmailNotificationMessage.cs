using NotificationService.Core.Extensions;
using NotificationService.Core.ValueObjects;

namespace NotificationService.Core.Models;

public sealed class EmailNotificationMessage
{
    public List<EmailAddress> To { get; }
    public List<EmailAddress> Cc { get; }
    public List<EmailAddress> Bcc { get; }
    public string Subject { get; }
    public string Content { get; }
    public bool IsHtml { get; }
    
    public const int MaxRecipients = 50;
    public const int MaxSubjectLength = 100;
    public const int MaxContentLengthInKB = 150;

    private EmailNotificationMessage()
    {
    }
    
    private EmailNotificationMessage(List<EmailAddress> to, string subject, string content, List<EmailAddress>? cc = null, List<EmailAddress>? bcc = null, bool isHtml = false)
    {
        To = to;
        Subject = subject;
        Content = content;
        Cc = cc ?? [];
        Bcc = bcc ?? [];
        IsHtml = isHtml;
    }

    public static EmailNotificationMessage Create(
        List<EmailAddress> to,
        string subject,
        string content,
        List<EmailAddress>? cc = null,
        List<EmailAddress>? bcc = null,
        bool isHtml = false)
    {
        ValidateEmailList(to);
        ValidateSubject(subject);
        ValidateContent(content);

        return new EmailNotificationMessage(to, subject, content, cc, bcc, isHtml);
    }

    private static void ValidateEmailList(ICollection<EmailAddress> to)
    {
        if (to.Count == 0)
        {
            throw new ArgumentException("At least one recipient is required");
        }
        
        if (to.Count > MaxRecipients)
        {
            throw new ArgumentException($"Maximum {MaxRecipients} recipients are allowed");
        }
    }

    private static void ValidateSubject(string subject)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException("Subject is required");
        }
        
        if (subject.Length > MaxSubjectLength)
        {
            throw new ArgumentException($"Subject must not exceed {MaxSubjectLength} characters");
        }
    }
    
    private static void ValidateContent(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentException("Content is required");
        }
        
        if (!content.HaveValidLengthInKB(MaxContentLengthInKB))
        {
            throw new ArgumentException($"Content must not exceed {MaxContentLengthInKB}KB");
        }
    }
}