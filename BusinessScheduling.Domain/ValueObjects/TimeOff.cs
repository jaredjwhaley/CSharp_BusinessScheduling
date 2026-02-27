namespace BusinessScheduling.Domain.ValueObjects;

public sealed class TimeOff : IEquatable<TimeOff>
{
    public DateTimeRange Range { get; }
    public TimeOffType Type { get; }

    public TimeOff(DateTimeRange range, TimeOffType type)
    {
        Range = range;
        Type = type;
    }

    public bool Equals(TimeOff? other)
    {
        if (other is null) return false;

        return Range.Equals(other.Range) &&
               Type == other.Type;
    }

    public override bool Equals(object? obj) => Equals(obj as TimeOff);

    public override int GetHashCode()
        => HashCode.Combine(Range, Type);
}
