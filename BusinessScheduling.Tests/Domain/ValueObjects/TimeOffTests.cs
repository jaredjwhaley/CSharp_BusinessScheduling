using BusinessScheduling.Domain.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public void Constructor_WhenTypeIsValid_CreatesInstance()
        {
            // Arrange
            var range = new DateTimeRange(
                DateTime.Parse("2026-02-28 10:00"),
                DateTime.Parse("2026-02-28 11:00"));
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
            var range = new DateTimeRange(
                DateTime.Parse("2026-02-28 10:00"),
                DateTime.Parse("2026-02-28 11:00"));
            var type = TimeOffType.Bereavement;

            var t1 = new TimeOff(range, type);
            var t2 = new TimeOff(range, type);

            // Act
            var areEqual = t1.Equals(t2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_WhenRangeDiffers_ReturnsFalse()
        {
            var t1 = new TimeOff(
                new DateTimeRange(
                    DateTime.Parse("2026-02-28 10:00"),
                    DateTime.Parse("2026-02-28 11:00")),
                TimeOffType.JuryDuty);

            var t2 = new TimeOff(
                new DateTimeRange(
                    DateTime.Parse("2026-02-28 11:00"),
                    DateTime.Parse("2026-02-28 12:00")),
                TimeOffType.JuryDuty);

            Assert.False(t1.Equals(t2));
        }

        [Fact]
        public void Equals_WhenTypeDiffers_ReturnsFalse()
        {
            var range = new DateTimeRange(
                DateTime.Parse("2026-02-28 10:00"),
                DateTime.Parse("2026-02-28 11:00"));

            var t1 = new TimeOff(range, TimeOffType.PaidTimeOff);
            var t2 = new TimeOff(range, TimeOffType.UnpaidLeave);

            Assert.False(t1.Equals(t2));
        }
        #endregion

        #region Theory with InlineData (Simple examples)
        [Theory]
        [InlineData("2026-02-28 10:00", "2026-02-28 11:00", "2026-02-28 10:00", "2026-02-28 11:00")]
        [InlineData("2026-02-28 09:00", "2026-02-28 10:00", "2026-02-28 09:00", "2026-02-28 10:00")]
        public void Equals_WithInlineData_ReturnsTrue(string s1, string e1, string s2, string e2)
        {
            // Arrange
            var t1 = new TimeOff(
                new DateTimeRange(DateTime.Parse(s1), DateTime.Parse(e1)),
                TimeOffType.PaidTimeOff);
            var t2 = new TimeOff(
                new DateTimeRange(DateTime.Parse(s2), DateTime.Parse(e2)),
                TimeOffType.PaidTimeOff);

            // Act & Assert
            Assert.Equal(t1, t2);
        }
        #endregion

        #region Theory with MemberData (Dynamic object arrays)

        // MemberData requires a static property, field, or method returning IEnumerable<object[]>
        // Each object[] corresponds to the parameters for one test case
        public static IEnumerable<object[]> TimeOffEqualityData =>
            new List<object[]>
            {
                new object[]
                {
                    new TimeOff(
                        new DateTimeRange(DateTime.Parse("2026-02-28 08:00"), DateTime.Parse("2026-02-28 09:00")),
                        TimeOffType.SickLeave),
                    new TimeOff(
                        new DateTimeRange(DateTime.Parse("2026-02-28 08:00"), DateTime.Parse("2026-02-28 09:00")),
                        TimeOffType.SickLeave),
                    true
                },
                new object[]
                {
                    new TimeOff(
                        new DateTimeRange(DateTime.Parse("2026-02-28 08:00"), DateTime.Parse("2026-02-28 09:00")),
                        TimeOffType.PaidTimeOff),
                    new TimeOff(
                        new DateTimeRange(DateTime.Parse("2026-02-28 08:00"), DateTime.Parse("2026-02-28 09:30")),
                        TimeOffType.PaidTimeOff),
                    false
                }
            };

        [Theory]
        [MemberData(nameof(TimeOffEqualityData))]
        public void Equals_WithMemberData(TimeOff t1, TimeOff t2, bool expected)
        {
            // Act
            var result = t1.Equals(t2);

            // Assert
            Assert.Equal(expected, result);
        }
        #endregion

        #region ClassData Example (Dynamic generation for all TimeOffTypes)

        // This class implements IEnumerable<object[]> and dynamically generates a TimeOff for each enum value
        public class AllTimeOffTypesProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (TimeOffType type in Enum.GetValues(typeof(TimeOffType)))
                {
                    // Here we're generating a TimeOff instance for each type dynamically
                    yield return new object[]
                    {
                        new TimeOff(
                            new DateTimeRange(
                                DateTime.Parse("2026-02-28 10:00"),
                                DateTime.Parse("2026-02-28 11:00")),
                            type)
                    };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(AllTimeOffTypesProvider))]
        public void Constructor_ForAllTimeOffTypes_CreatesInstance(TimeOff timeOff)
        {
            // This demonstrates how ClassData can dynamically supply complex objects to your tests
            Assert.NotNull(timeOff);
            Assert.IsType<TimeOff>(timeOff);
        }
        #endregion
    }
}
