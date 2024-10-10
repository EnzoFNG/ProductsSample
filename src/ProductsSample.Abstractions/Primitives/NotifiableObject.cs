using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProductsSample.Abstractions.Primitives;

public abstract class NotifiableObject<T> : Notifiable<Notification>
    where T : Notifiable<Notification>
{
    [BindNever]
    protected static Contract<T> ContractRequires => new Contract<T>().Requires();

    [BindNever]
    protected string Key => GetType().Name;

    protected internal abstract void Validate();

    public void AddNotificationsIfNotNull<N>(params Notifiable<N>?[] items) where N : Notification
    {
        foreach (Notifiable<N>? item in items)
        {
            if (item is null)
                continue;

            AddNotifications(item.Notifications);
        }
    }
}