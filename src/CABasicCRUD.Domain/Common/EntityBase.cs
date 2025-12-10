namespace CABasicCRUD.Domain.Common;

public abstract class EntityBase<TId> : IEquatable<EntityBase<TId>>
    where TId : EntityIdBase
{
    public TId Id { get; protected set; }

    protected EntityBase(TId? id)
    {
        if (id is null)
        {
            throw new ArgumentException("Entity Id cannot be a null value", nameof(id));
        }
        Id = id;
    }

    public bool Equals(EntityBase<TId>? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as EntityBase<TId>);
    }

    public override int GetHashCode()
    {
        return Id.Value.GetHashCode();
    }

    public static bool operator ==(EntityBase<TId>? left, EntityBase<TId>? right)
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(EntityBase<TId>? left, EntityBase<TId>? right) =>
        !(left == right);
}
