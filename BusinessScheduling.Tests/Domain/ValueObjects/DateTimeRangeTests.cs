using BusinessScheduling.Domain.ValueObjects;
using System.Globalization;
using Xunit;

namespace BusinessScheduling.Tests.Domain.ValueObjects;

public class DateTimeRangeTests
{
    [Fact]
    public void Constructor_WhenStartIsEqualToEnd_ThrowsArgumentException()
    {
        // Arrange
        var start = DateTime.ParseExact("2026-02-28 18:42:00,000", "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
        var end = start;

        // Act
        // NOTE: Assert.Throws requires an Action delegate, so we use a lambda expression to create it; using the
        // standard method group syntax (e.g., Assert.Throws<ArgumentException>(new DateTimeRange)) would not work here
        // because the constructor requires parameters.
        Action act = () => new DateTimeRange(start, end);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Constructor_WhenStartIsGreaterThanEnd_ThrowsArgumentException()
    {
        // Arrange
        var end = DateTime.ParseExact("2026-02-28 18:42:00,000", "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
        var start = end.AddSeconds(1);

        // Act
        Action act = () => new DateTimeRange(start, end);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Constructor_WhenStartIsBeforeEnd_CreatesInstance()
    {
        // Arrange
        var start = DateTime.ParseExact("2026-02-28 18:42:00,000", "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
        var end = start.AddMinutes(1);

        // Act
        var range = new DateTimeRange(start, end);

        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }
}
