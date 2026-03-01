using BusinessScheduling.Domain.ValueObjects;
using System.Collections;
using Xunit;

namespace BusinessScheduling.Tests.Domain.ValueObjects
{
    public class TimeOffTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_WhenRangeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            DateTimeRange? range = null;
            var type = TimeOffType.PaidTimeOff;

            // Act
            Action act = () => new TimeOff(range!, type); // '!' suppresses nullable warning for testing

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Constructor_WhenGivenValidRangeAndType_CreatesInstance()
        {
            // Arrange
            var range = new DateTimeRange(DateTime.Now, DateTime.Now.AddHours(4));
            var type = TimeOffType.SickLeave;

            // Act
            var timeOff = new TimeOff(range, type);

            // Assert
            Assert.Equal(range, timeOff.Range);
            Assert.Equal(type, timeOff.Type);
        }

        #endregion

        #region Equality Tests

        [Fact]
        public void Equals_WhenRangeAndTypeMatch_ReturnsTrue()
        {
            // Arrange
            var range = new DateTimeRange(DateTime.Now, DateTime.Now.AddHours(4));
            var type = TimeOffType.PaidTimeOff;
            var timeOff1 = new TimeOff(range, type);
            var timeOff2 = new TimeOff(range, type);

            // Act
            var result = timeOff1.Equals(timeOff2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WhenRangeDiffers_ReturnsFalse()
        {
            // Arrange
            var range1 = new DateTimeRange(DateTime.Now, DateTime.Now.AddHours(4));
            var range2 = new DateTimeRange(DateTime.Now, DateTime.Now.AddHours(5));
            var type = TimeOffType.PaidTimeOff;

            var timeOff1 = new TimeOff(range1, type);
            var timeOff2 = new TimeOff(range2, type);

            // Act & Assert
            Assert.False(timeOff1.Equals(timeOff2));
        }

        [Fact]
        public void Equals_WhenTypeDiffers_ReturnsFalse()
        {
            // Arrange
            var range = new DateTimeRange(DateTime.Now, DateTime.Now.AddHours(4));
            var timeOff1 = new TimeOff(range, TimeOffType.PaidTimeOff);
            var timeOff2 = new TimeOff(range, TimeOffType.SickLeave);

            // Act & Assert
            Assert.False(timeOff1.Equals(timeOff2));
        }

        #endregion

        #region MemberData Testing Example

        // MemberData allows us to provide complex or reusable datasets for a Theory.
        // Here we define a static property returning IEnumerable<object[]>.
        // Each object[] represents a separate set of parameters for the test method.
        public static IEnumerable<object[]> TimeOffData =>
            new List<object[]>
            {
                new object[] { new DateTimeRange(DateTime.Parse("2026-02-28 09:00"), DateTime.Parse("2026-02-28 13:00")), TimeOffType.PaidTimeOff },
                new object[] { new DateTimeRange(DateTime.Parse("2026-02-28 10:00"), DateTime.Parse("2026-02-28 14:00")), TimeOffType.SickLeave },
                new object[] { new DateTimeRange(DateTime.Parse("2026-02-28 08:00"), DateTime.Parse("2026-02-28 12:00")), TimeOffType.JuryDuty }
            };

        [Theory]
        [MemberData(nameof(TimeOffData))]
        public void Constructor_WithMemberData_CreatesValidInstance(DateTimeRange range, TimeOffType type)
        {
            var timeOff = new TimeOff(range, type);

            Assert.Equal(range, timeOff.Range);
            Assert.Equal(type, timeOff.Type);
        }

        #endregion

        #region ClassData Testing Example (Dynamic Enum Coverage)

        // ClassData is like MemberData but encapsulates the dataset in a separate class.
        // Here, we dynamically provide one instance for each TimeOffType in the enum.
        public class TimeOffTypeProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (TimeOffType type in Enum.GetValues(typeof(TimeOffType)))
                {
                    // For each TimeOffType, create a simple valid DateTimeRange and supply as object[]
                    // NOTE: "yield" allows us to return each item one at a time without needing to build the entire list in memory.
                    yield return new object[]
                    {
                        // For testing purposes, we can use the same DateTimeRange for all types since we're focusing on enum coverage.
                        new DateTimeRange(DateTime.Today, DateTime.Today.AddHours(4)),
                        // The TimeOffType value from the loop.
                        type
                    };
                }
            }

            // This non-generic GetEnumerator is required by IEnumerable. It simply calls the generic version.
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(TimeOffTypeProvider))]
        public void Constructor_WithClassData_CreatesInstanceForAllEnumValues(DateTimeRange range, TimeOffType type)
        {
            // This test runs once for every TimeOffType value, so we dynamically verify coverage
            var timeOff = new TimeOff(range, type);

            Assert.Equal(range, timeOff.Range);
            Assert.Equal(type, timeOff.Type);
        }

        #endregion
    }
}
