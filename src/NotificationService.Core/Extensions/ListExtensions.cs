namespace NotificationService.Core.Extensions;

public static class ListExtensions
{
    public static IEnumerable<List<T>> SplitList<T>(this List<T> items, int size)
    {
        for (var i = 0; i < items.Count; i += size) yield return items.GetRange(i, Math.Min(size, items.Count - i));
    }
}