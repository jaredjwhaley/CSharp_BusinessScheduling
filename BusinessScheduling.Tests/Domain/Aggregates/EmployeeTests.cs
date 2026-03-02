using BusinessScheduling.Domain.Aggregates;
using BusinessScheduling.Domain.ValueObjects;
using Xunit;

namespace BusinessScheduling.Tests.Domain.Aggregates
{
    public class EmployeeTests
    {
        private readonly Guid _personId = Guid.NewGuid();

        private static void LogFortyHourWeek(Employee employee)
        {
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-02 09:00"),
                DateTime.Parse("2026-03-02 17:00"))); // 8h
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-03 09:00"),
                DateTime.Parse("2026-03-03 17:00"))); // 8h
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-04 09:00"),
                DateTime.Parse("2026-03-04 17:00"))); // 8h
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-05 09:00"),
                DateTime.Parse("2026-03-05 17:00"))); // 8h
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-06 09:00"),
                DateTime.Parse("2026-03-06 17:00"))); // 8h
        }

        private static void LogSixtyHourWeek(Employee employee)
        {
            LogFortyHourWeek(employee);
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-02 17:00"),
                DateTime.Parse("2026-03-02 21:00"))); // 4h
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-03 17:00"),
                DateTime.Parse("2026-03-03 21:00"))); // 4h
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-04 17:00"),
                DateTime.Parse("2026-03-04 21:00"))); // 4h
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-05 17:00"),
                DateTime.Parse("2026-03-05 21:00"))); // 4h
            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-06 17:00"),
                DateTime.Parse("2026-03-06 21:00"))); // 4h
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WhenPersonIdIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Employee(Guid.Empty, 25m, CompensationType.Hourly));
        }

        [Fact]
        public void Constructor_WhenHourlyRateIsNegative_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Employee(_personId, -1m, CompensationType.Hourly));
        }

        [Fact]
        public void Constructor_WhenValid_CreatesActiveEmployee()
        {
            var employee = new Employee(_personId, 0m, CompensationType.Hourly);

            Assert.NotEqual(Guid.Empty, employee.Id);
            Assert.Equal(_personId, employee.PersonId);
            Assert.True(employee.IsActive);
        }

        #endregion

        #region Activation Lifecycle Tests

        [Fact]
        public void Deactivate_SetsIsActiveToFalse()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);

            employee.Deactivate();

            Assert.False(employee.IsActive);
        }

        [Fact]
        public void Reactivate_SetsIsActiveToTrue()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);
            employee.Deactivate();

            employee.Reactivate();

            Assert.True(employee.IsActive);
        }

        #endregion

        #region TimeOff Invariant Tests

        [Fact]
        public void AddTimeOff_WhenRangesOverlap_ThrowsInvalidOperationException()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);

            var first = new TimeOff(
                new DateTimeRange(
                    DateTime.Parse("2026-03-01 09:00"),
                    DateTime.Parse("2026-03-01 17:00")),
                TimeOffType.PaidTimeOff);

            var overlapping = new TimeOff(
                new DateTimeRange(
                    DateTime.Parse("2026-03-01 13:00"),
                    DateTime.Parse("2026-03-01 18:00")),
                TimeOffType.SickLeave);

            employee.AddTimeOff(first);

            Assert.Throws<InvalidOperationException>(() =>
                employee.AddTimeOff(overlapping));
        }

        [Fact]
        public void AddTimeOff_WhenNonOverlapping_AddsSuccessfully()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);

            var timeOff = new TimeOff(
                new DateTimeRange(
                    DateTime.Parse("2026-03-01 09:00"),
                    DateTime.Parse("2026-03-01 17:00")),
                TimeOffType.PaidTimeOff);

            employee.AddTimeOff(timeOff);

            Assert.Single(employee.TimeOff);
        }

        #endregion

        #region Logged Hours Invariant Tests

        [Fact]
        public void LogHours_WhenRangesOverlap_ThrowsInvalidOperationException()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);

            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-02 09:00"),
                DateTime.Parse("2026-03-02 12:00")));

            Assert.Throws<InvalidOperationException>(() =>
                employee.LogHours(new DateTimeRange(
                    DateTime.Parse("2026-03-02 11:00"),
                    DateTime.Parse("2026-03-02 14:00"))));
        }

        [Fact]
        public void LogHours_WhenNonOverlapping_AddsSuccessfully()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);

            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-02 09:00"),
                DateTime.Parse("2026-03-02 12:00")));

            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-02 13:00"),
                DateTime.Parse("2026-03-02 17:00")));

            Assert.Equal(2, employee.LoggedHours.Count);
        }

        #endregion

        #region Payroll Behavior Tests

        [Fact]
        public void UpdateHourlyRate_ValidRate_UpdatesSuccessfully()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);
            employee.UpdateHourlyRate(25m);
            Assert.Equal(25m, employee.HourlyRate);
        }

        [Fact]
        public void UpdateRate_ValidRate_ChangesPayoutCalculation()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);
            LogFortyHourWeek(employee);
            var payPeriod = new DateTimeRange(
                DateTime.Parse("2026-03-01 00:00"),
                DateTime.Parse("2026-03-07 23:59"));
            var originalPayout = employee.CalculatePayout(payPeriod);
            employee.UpdateHourlyRate(25m);
            var updatedPayout = employee.CalculatePayout(payPeriod);
            Assert.NotEqual(originalPayout, updatedPayout);
        }

        [Fact]
        public void UpdateHourlyRate_NegativeRate_ThrowsArgumentException()
        {
            var employee = new Employee(_personId, 20m, CompensationType.Hourly);
            Assert.Throws<ArgumentException>(() => employee.UpdateHourlyRate(-5m));
        }

        [Fact]
        public void CalculatePayout_ForHourlyEmployee_WithOvertime_PaysTimeAndAHalf()
        {
            var hourlyRate = 10m;
            var employee = new Employee(_personId, hourlyRate, CompensationType.Hourly);
            LogSixtyHourWeek(employee);

            var payPeriod = new DateTimeRange(
                DateTime.Parse("2026-03-01 00:00"),
                DateTime.Parse("2026-03-07 23:59"));

            var payout = employee.CalculatePayout(payPeriod);

            // 40 regular + 20 overtime
            Assert.Equal((40 * hourlyRate) + (20 * hourlyRate * 1.5m), payout);
        }

        [Fact]
        public void CalculatePayout_ForSalaryEmployee_IgnoresLoggedHours()
        {
            var employee = new Employee(_personId, 25m, CompensationType.Salary);

            employee.LogHours(new DateTimeRange(
                DateTime.Parse("2026-03-01 08:00"),
                DateTime.Parse("2026-03-01 20:00")));

            var payPeriod = new DateTimeRange(
                DateTime.Parse("2026-03-01"),
                DateTime.Parse("2026-03-08"));

            var payout = employee.CalculatePayout(payPeriod);

            Assert.Equal(25m * 40, payout);
        }

        #endregion
    }
}