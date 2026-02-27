using System;

namespace BusinessScheduling.Domain.ValueObjects
{
    /// <summary>
    /// Represents a period of time with a start and end.
    /// </summary>
    public sealed class TimeRange : IEquatable<TimeRange>
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        /// <summary>
        /// Constructs a TimeRange with validation.
        /// Start must be strictly before End.
        /// </summary>
        /// <param name="start">Start of the time range.</param>
        /// <param name="end">End of the time range.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if Start greater than or equal to End.
        /// </exception>
        public TimeRange(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                throw new ArgumentException("Start time must be before end time.");
            }

            Start = start;
            End = end;
        }

        /// <summary>
        /// Checks if this TimeRange overlaps with another.
        /// Useful for schedule conflict detection.
        /// </summary>
        public bool Overlaps(TimeRange other)
        {
            return Start < other.End && End > other.Start;
        }

        #region Equality by Value

        /// <summary>
        /// Determines whether the specified <see cref="TimeRange"/> instance is equal to the current instance based on
        /// their start and end values.
        /// </summary>
        /// <remarks>This method performs a value-based equality check, comparing the <see cref="Start"/>
        /// and <see cref="End"/> properties of both <see cref="TimeRange"/> instances.</remarks>
        /// <param name="other">The <see cref="TimeRange"/> instance to compare with the current instance. If <paramref name="other"/> is
        /// <see langword="null"/>, the method returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if the specified <see cref="TimeRange"/> instance is equal to the current instance;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(TimeRange? other)
        {
            if (other is null) return false;
            return Start == other.Start && End == other.End;
        }

        public override bool Equals(object? obj) => Equals(obj as TimeRange);

        public override int GetHashCode() => HashCode.Combine(Start, End);

        #endregion
    }
}
