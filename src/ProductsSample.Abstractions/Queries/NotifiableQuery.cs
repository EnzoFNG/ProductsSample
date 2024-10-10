using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProductsSample.Abstractions.Queries;

public abstract class NotifiableQuery<T> where T : Notification
{
    private readonly List<T> _notifications;

    [BindNever]
    public IReadOnlyCollection<T> Notifications => _notifications;

    [BindNever]
    public bool IsValid => _notifications.Count == 0;

    [BindNever]
    protected string Key => GetType().Name;

    [BindNever]
    protected static Contract<T> ContractRequires => new Contract<T>().Requires();

    protected NotifiableQuery()
    {
        _notifications = [];
    }

    public abstract void Validate();

    private static T GetNotificationInstance(string key, string message)
    {
        return (T)Activator.CreateInstance(typeof(T), key, message)!;
    }

    public void AddNotification(string key, string message)
    {
        T notificationInstance = NotifiableQuery<T>.GetNotificationInstance(key, message);
        _notifications.Add(notificationInstance);
    }

    public void AddNotification(T notification)
    {
        _notifications.Add(notification);
    }

    public void AddNotification(Type property, string message)
    {
        T notificationInstance = NotifiableQuery<T>.GetNotificationInstance(property?.Name!, message);
        _notifications.Add(notificationInstance);
    }

    public void AddNotifications(IReadOnlyCollection<T> notifications)
    {
        _notifications.AddRange(notifications);
    }

    public void AddNotifications(IList<T> notifications)
    {
        _notifications.AddRange(notifications);
    }

    public void AddNotifications(ICollection<T> notifications)
    {
        _notifications.AddRange(notifications);
    }

    public void AddNotifications(Notifiable<T> item)
    {
        AddNotifications(item.Notifications);
    }

    public void AddNotifications(params Notifiable<T>[] items)
    {
        foreach (Notifiable<T> item in items)
        {
            AddNotifications(item);
        }
    }

    public void Clear()
    {
        _notifications.Clear();
    }
}