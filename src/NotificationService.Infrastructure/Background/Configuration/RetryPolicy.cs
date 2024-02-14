namespace NotificationService.Infrastructure.Background.Configuration;

public class RetryPolicy
{
    public int MaxRetries { get; set; } = 3;
    public TimeSpan DelayBetweenRetries { get; set; } = TimeSpan.FromSeconds(5);
}