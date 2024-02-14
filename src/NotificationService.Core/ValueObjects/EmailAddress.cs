using System.Text.RegularExpressions;

namespace NotificationService.Core.ValueObjects;

public class EmailAddress : ValueObject<string>
{
    private const string EmailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    private EmailAddress()
    {
    }

    public EmailAddress(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid email address");
        }

        Value = value;
    }

    private static bool IsValid(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email), "Email address is required");
        }
        
        return Regex.IsMatch(email, EmailRegexPattern, RegexOptions.IgnoreCase);
    }
}