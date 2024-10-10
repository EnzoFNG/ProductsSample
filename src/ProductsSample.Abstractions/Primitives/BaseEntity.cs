using ProductsSample.Abstractions.Enums;

namespace ProductsSample.Abstractions.Primitives;

public abstract class BaseEntity<T> : NotifiableObject<T>, IEqualityComparer<BaseEntity<T>> where T : BaseEntity<T>
{
    protected BaseEntity() : base()
    {
        Id = Ulid.NewUlid();
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        Status = EntityStatus.Active;
    }

    public Ulid Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    public DateTime UpdatedAt { get; protected set; }

    public EntityStatus Status { get; protected set; }

    public virtual bool Activate()
    {
        if (!IsValid)
            return false;

        Status = EntityStatus.Active;
        return true;
    }

    public virtual bool Deactivate()
    {
        if (!IsValid)
            return false;

        Status = EntityStatus.Inactive;
        return true;
    }

    #region Equality
    public override bool Equals(object? obj)
        => obj is BaseEntity<T> entity && Id!.Equals(entity.Id);

    public bool Equals(BaseEntity<T>? x, BaseEntity<T>? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null)
            return false;

        if (y is null)
            return false;

        if (x.GetType() != y.GetType())
            return false;

        return x.Id.Equals(y.Id)
            && x.Status == y.Status;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, Status);

    public int GetHashCode(BaseEntity<T> obj)
        => HashCode.Combine(obj.Id, obj.Status);
    #endregion
}