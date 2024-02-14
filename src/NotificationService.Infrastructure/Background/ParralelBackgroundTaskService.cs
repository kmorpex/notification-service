using Microsoft.Extensions.Hosting;
using NotificationService.Infrastructure.Background.Configuration;

namespace NotificationService.Infrastructure.Background;

public class ParralelBackgroundTaskService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly RetryPolicy _retryPolicy;
    private readonly int _workerCount;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public ParralelBackgroundTaskService(IBackgroundTaskQueue taskQueue, RetryPolicy retryPolicy, WorkerConfiguration workerConfiguration)
    {
        _taskQueue = taskQueue;
        _retryPolicy = retryPolicy;
        _workerCount = workerConfiguration.WorkerCount;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tasks = new Task[_workerCount];

        for (int i = 0; i < _workerCount; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var priorityTask = await _taskQueue.DequeueAsync(_cancellationTokenSource.Token);

                    int retryCount = 0;

                    while (retryCount < _retryPolicy.MaxRetries)
                    {
                        try
                        {
                            await priorityTask.TaskFunction(_cancellationTokenSource.Token);
                            break;
                        }
                        catch (Exception)
                        {
                            // Log the exception if necessary
                            retryCount++;

                            if (retryCount < _retryPolicy.MaxRetries)
                            {
                                await Task.Delay(_retryPolicy.DelayBetweenRetries, _cancellationTokenSource.Token);
                            }
                        }
                    }
                }
            }, stoppingToken);
        }

        await Task.WhenAll(tasks);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _cancellationTokenSource.CancelAsync();
        await base.StopAsync(cancellationToken);
    }
}