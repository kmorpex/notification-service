namespace NotificationService.Core.Extensions;

public static class StringExtensions
{
    public static bool HaveValidLengthInBytes(this string content, int maxSizeInBytes)
    {
        int contentSizeInBytes = System.Text.Encoding.UTF8.GetByteCount(content);
        return contentSizeInBytes <= maxSizeInBytes;
    }
    
    public static bool HaveValidLengthInKB(this string content, int maxSizeInKB)
    {
        int maxSizeInBytes = maxSizeInKB * 1024;
        return content.HaveValidLengthInBytes(maxSizeInBytes);
    }
}