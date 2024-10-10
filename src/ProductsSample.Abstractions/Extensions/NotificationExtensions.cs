using Flunt.Notifications;

namespace ProductsSample.Abstractions.Extensions;

public static class NotificationExtensions
{
    protected sealed record ErrorResponse(string Key, string Message);

    public static string ToListString(this IReadOnlyCollection<Notification> notifications)
    {
        var errors = notifications.Select(x => new ErrorResponse(x.Key, x.Message));

        return new Dictionary<string, ErrorResponse[]>
            {
                { $"errors", errors.ToArray() }
            }
        .ToString()!;
    }
}