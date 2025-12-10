namespace CABasicCRUD.Domain.Common;

public abstract record EntityIdBase
{
    public Guid Value { get; init; }

    protected EntityIdBase(Guid guid)
    {
        if (guid == default)
        {
            throw new ArgumentException("Entity Id cannot be a default value.", nameof(guid));
        }
        Value = guid;
    }

    public static implicit operator Guid(EntityIdBase entityIdBase) => entityIdBase.Value;
}
