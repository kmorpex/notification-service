namespace NotificationService.Infrastructure.Exceptions;

public class RetryableException : Exception
{
    public RetryableException(string? message) : base(message)
    {
    }
    
    public RetryableException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}