namespace BusinessScheduling.Domain.ValueObjects;

/// <summary>
/// Represents a period of time off, including its duration and type.
/// </summary>
/// <remarks>The TimeOff class encapsulates a <see cref="DateTimeRange"/> and the <see cref="TimeOffType"/>, such as vacation or sick
/// leave. It implements equality comparison based on the range and type.</remarks>
public sealed class TimeOff : IEquatable<TimeOff>
{
    public DateTimeRange Range { get; }
    public TimeOffType Type { get; }

    /// <summary>
    /// Initializes a new instance of the TimeOff class for the specified date range and type of absence.
    /// </summary>
    /// <remarks>Both parameters must be valid to ensure the TimeOff instance is properly
    /// initialized.</remarks>
    /// <param name="range">The date range that defines the period of time off. This parameter cannot be null.</param>
    /// <param name="type">The type of time off being requested, which determines the nature of the absence.</param>
    public TimeOff(DateTimeRange range, TimeOffType type)
    {
        if (range is null)
            throw new ArgumentNullException(nameof(range), "TimeOff must have a valid date range.");
        Range = range;
        Type = type;
    }

    #region Equality by Value

    public bool Equals(TimeOff? other)
    {
        if (other is null) return false;

        return Range.Equals(other.Range) &&
               Type == other.Type;
    }

    public override bool Equals(object? obj) => Equals(obj as TimeOff);

    public override int GetHashCode()
        => HashCode.Combine(Range, Type);

    #endregion
}
