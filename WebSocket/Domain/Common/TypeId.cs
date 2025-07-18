namespace WebSocket.Domain.Common;

public class TypeId<T>(Guid value)
    where T : Entity<T>
{
    public Guid Value { get; init; } = value;
    public static TypeId<T> New() => new(Guid.NewGuid());
    public static TypeId<T> Empty() => new(Guid.Empty);
}