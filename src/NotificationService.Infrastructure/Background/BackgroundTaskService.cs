using Microsoft.Extensions.Hosting;
using NotificationService.Infrastructure.Background.Configuration;
using NotificationService.Infrastructure.Exceptions;

namespace NotificationService.Infrastructure.Background;

public class BackgroundTaskService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly RetryPolicy _retryPolicy;

    public BackgroundTaskService(IBackgroundTaskQueue taskQueue, RetryPolicy retryPolicy)
    {
        _taskQueue = taskQueue;
        _retryPolicy = retryPolicy;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var priorityTask = await _taskQueue.DequeueAsync(stoppingToken);
            if (priorityTask == null) continue;

            int retryCount = 0;

            while (retryCount < _retryPolicy.MaxRetries)
            {
                try
                {
                    await priorityTask.TaskFunction(stoppingToken);
                    break;
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary
                    Console.WriteLine($"Error executing background background task: {ex.Message}");
                    
                    if (ex is not RetryableException)
                    {
                        break;
                    } 
                    
                    retryCount++;

                    if (retryCount < _retryPolicy.MaxRetries)
                    {
                        await Task.Delay(_retryPolicy.DelayBetweenRetries, stoppingToken);
                    }
                }
            }
        }
    }
}