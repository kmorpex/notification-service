using System.Text.RegularExpressions;

namespace NotificationService.Core.ValueObjects;

public class PhoneNumber : ValueObject<string>
{
    private const string PhoneNumberE164RegexPattern = @"^\+[1-9]\d{1,14}$";

    private PhoneNumber()
    {
    }

    public PhoneNumber(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid phone number");
        }

        Value = value;
    }

    private static bool IsValid(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            throw new ArgumentNullException(nameof(phoneNumber), "Phone number is required");
        }

        return Regex.IsMatch(phoneNumber, PhoneNumberE164RegexPattern, RegexOptions.IgnoreCase);
    }
}
