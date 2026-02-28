using BusinessScheduling.Domain.ValueObjects;

namespace BusinessScheduling.Domain.Aggregates;

public sealed class Employee
{
    public Guid Id { get; private set; }
    public Guid PersonId { get; private set; }

    public decimal HourlyRate { get; private set; }
    public CompensationType CompensationType { get; private set; }

    public bool IsActive { get; private set; }

    private readonly List<TimeOff> _timeOff = new();
    public IReadOnlyCollection<TimeOff> TimeOff => _timeOff.AsReadOnly();

    public Employee(
        Guid personId,
        decimal hourlyRate,
        CompensationType compensationType,
        Guid? id = null)
    {
        if (personId == Guid.Empty)
            throw new ArgumentException("Employee must reference a valid Person.");

        if (hourlyRate < 0)
            throw new ArgumentException("Hourly rate must be greater than or equal to zero.");

        Id = id ?? Guid.NewGuid();
        PersonId = personId;
        HourlyRate = hourlyRate;
        CompensationType = compensationType;
        IsActive = true;
    }

    public void AddTimeOff(TimeOff timeOff)
    {
        if (_timeOff.Any(t => t.Range.Overlaps(timeOff.Range)))
            throw new InvalidOperationException("Time off overlaps an existing absence.");

        _timeOff.Add(timeOff);
    }

    #region Payroll Calculations

    public decimal CalculatePayout(DateTimeRange period)
    {
        return CompensationType switch
        {
            CompensationType.Hourly => CalculateHourlyPayout(period),
            CompensationType.Salary => CalculateSalaryPayout(period),
            _ => throw new InvalidOperationException("Unknown compensation type.")
        };
    }

    private decimal CalculateHourlyPayout(DateTimeRange period)
    {
        var totalHours = period.Duration.TotalHours;

        var unpaidHours = _timeOff
            .Where(t => t.Type is TimeOffType.UnpaidLeave or TimeOffType.NoShow)
            .Where(t => t.Range.Overlaps(period))
            .Sum(t =>
            {
                var overlapStart = period.Start.CompareTo(t.Range.Start) >= 0 ? period.Start : t.Range.Start;
                var overlapEnd = period.End.CompareTo(t.Range.End) >= 0 ? period.End : t.Range.End;
                return (overlapEnd - overlapStart).TotalHours;
            });

        var payableHours = Math.Max(0, totalHours - unpaidHours);
        return (decimal)payableHours * HourlyRate;
    }

    private decimal CalculateSalaryPayout(DateTimeRange period)
    {
        var year = period.Start.Year;
        var daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;

        var weeklySalary = HourlyRate * 40m;
        var dailyRate = weeklySalary / 7m;

        var totalDays = period.Duration.TotalDays;
        return dailyRate * (decimal)totalDays;
    }

    #endregion

    public void Deactivate() => IsActive = false;
    public void Reactivate() => IsActive = true;
}
