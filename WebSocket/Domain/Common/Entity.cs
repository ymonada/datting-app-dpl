namespace WebSocket.Domain.Common;

public abstract class Entity<TId> : IEquatable<Entity<TId>> 
    where  TId : notnull
{
    public readonly TId Id;

    protected Entity(TId id)
    {
        Id = id;
    }
    public bool Equals(object? obj) =>  obj is Entity<TId> other && Equals(other);
    public bool Equals(Entity<TId>? other) => Equals((object?)other);
    public static bool operator ==(Entity<TId> left, Entity<TId> right) => left.Equals(right);
    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !left.Equals(right);
    public override int GetHashCode()=> Id.GetHashCode();
    
}