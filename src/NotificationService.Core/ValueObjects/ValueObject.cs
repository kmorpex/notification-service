using System.Reflection;

namespace NotificationService.Core.ValueObjects;

public class ValueObject<TValue> : IEquatable<ValueObject<TValue>>
{
    //public abstract IEnumerable<object> GetEqualityComponents();

    protected ValueObject()
    {
    }

    // public ValueObject()
    // {
    // }

    public ValueObject(TValue value)
    {
        Value = value;
    }

    public TValue? Value { get; protected set; }

    public bool Equals(ValueObject<TValue>? other)
    {
        return Equals((object?)other);
    }

    public static IEnumerable<T> GetAll<T>() where T : ValueObject<TValue>
    {
        return typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Select(f => f.GetValue(null)).Cast<T>();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var other = (ValueObject<TValue>)obj;
        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject<TValue> left, ValueObject<TValue> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject<TValue>? left, ValueObject<TValue> right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static implicit operator TValue(ValueObject<TValue> value)
    {
        return value.Value;
    }

    public static implicit operator ValueObject<TValue>(TValue value)
    {
        return new ValueObject<TValue>(value);
    }

    protected virtual IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }
}