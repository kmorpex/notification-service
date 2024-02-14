using System.Collections.Concurrent;
using NotificationService.Infrastructure.Background.Configuration;
using NotificationService.Infrastructure.Background.Shared;

namespace NotificationService.Infrastructure.Background;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly List<ConcurrentQueue<PriorityBackgroundTask>> _queues;
    private readonly SemaphoreSlim _signal;
    private int _currentIndex;

    public BackgroundTaskQueue(WorkerConfiguration workerConfiguration)
    {
        _queues = Enumerable.Range(0, workerConfiguration.WorkerCount)
            .Select(_ => new ConcurrentQueue<PriorityBackgroundTask>())
            .ToList();
        _signal = new SemaphoreSlim(workerConfiguration.WorkerCount);
        _currentIndex = 0;
    }

    public void QueueBackgroundWorkItem(int priority, BackgroundWorkItemDelegate workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        var priorityTask = new PriorityBackgroundTask { TaskFunction = workItem, Priority = priority };
        _queues[GetNextIndex()].Enqueue(priorityTask);
        _signal.Release();
    }

    public async Task<PriorityBackgroundTask> DequeueAsync(CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);

        foreach (var t in _queues)
        {
            if (t.TryDequeue(out var priorityTask))
            {
                return priorityTask;
            }
        }

        return null!;
    }

    private int GetNextIndex()
    {
        int index = _currentIndex;
        _currentIndex = (_currentIndex + 1) % _queues.Count;
        return index;
    }
}