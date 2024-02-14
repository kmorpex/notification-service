namespace NotificationService.Infrastructure.Exceptions;

public class NonRetryableException : Exception
{
    public NonRetryableException(string? message) : base(message)
    {
    }
    
    public NonRetryableException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}