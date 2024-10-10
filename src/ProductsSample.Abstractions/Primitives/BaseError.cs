using Flunt.Notifications;

namespace ProductsSample.Abstractions.Primitives;

public abstract class BaseError : Notification
{
    protected BaseError(string message)
    {
        Key = GetType().Name;
        Message = message;
    }
}