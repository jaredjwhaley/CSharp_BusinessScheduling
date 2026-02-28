using BusinessScheduling.Domain.ValueObjects;

namespace BusinessScheduling.Domain.Aggregates;

/// <summary>
/// Represents an employee record associated with a person.
/// </summary>
/// <remarks>The Employee class manages <see cref="CompensationType">compensation type</see>, hourly rate, and time off for an individual employee.
/// It provides methods to calculate payouts based on <see cref="CompensationType">compensation type</see> and ensures that time off requests do not
/// overlap. Use this class to track employment status and manage payroll calculations for employees within the business
/// domain.</remarks>
public sealed class Employee
{
    public Guid Id { get; private set; }
    /// <summary>
    /// Gets the unique identifier for the person that the employee is linked to.
    /// </summary>
    /// <remarks>The PersonId is assigned upon creation and cannot be modified thereafter.</remarks>
    public Guid PersonId { get; private set; }
    /// <summary>
    /// Gets the hourly rate for the employee.
    /// </summary>
    /// <remarks>This is used in payout calculation for all <see cref="CompensationType">compensation types</see>.</remarks>
    public decimal HourlyRate { get; private set; }
    public CompensationType CompensationType { get; private set; }
    /// <summary>
    /// Gets a value indicating whether the employee is currently employed with the business.
    /// </summary>
    public bool IsActive { get; private set; }

    private readonly List<TimeOff> _timeOff = new();
    private readonly List<DateTimeRange> _loggedHours = new();
    public IReadOnlyCollection<TimeOff> TimeOff => _timeOff.AsReadOnly();
    public IReadOnlyCollection<DateTimeRange> LoggedHours => _loggedHours.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the Employee class with the specified person identifier, hourly rate, compensation
    /// type, and an optional employee identifier.
    /// </summary>
    /// <param name="personId">The unique identifier of the person associated with the employee. This value must be a non-empty GUID.</param>
    /// <param name="hourlyRate">The hourly pay rate for the employee. Must be greater than or equal to zero.</param>
    /// <param name="compensationType">The <see cref="CompensationType">compensation type</see> that specifies how the employee is paid.</param>
    /// <param name="id">An optional unique identifier for the employee (only provided for data rehydration). If not provided, a new identifier is generated.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="personId"/> is an empty GUID or if <paramref name="hourlyRate"/> is less than zero.</exception>
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

    /// <summary>
    /// Adds a <see cref="TimeOff"/> entry to the collection, ensuring that it does not overlap with any existing time off entries.
    /// </summary>
    /// <param name="timeOff">The time off entry to add. The time range of this request must not overlap with any existing time off periods.</param>
    /// <exception cref="InvalidOperationException">Thrown if the provided time off entry overlaps with an existing time off entry.</exception>
    public void AddTimeOff(TimeOff timeOff)
    {
        if (_timeOff.Any(t => t.Range.Overlaps(timeOff.Range)))
            throw new InvalidOperationException("Time off overlaps an existing absence.");

        _timeOff.Add(timeOff);
    }

    /// <summary>
    /// Logs the hours worked within the specified date and time range.
    /// </summary>
    /// <remarks>This method ensures that no overlapping hours are logged to maintain accurate
    /// records.</remarks>
    /// <param name="dateTimeRange">The date and time range representing the hours worked. This range must not overlap with any previously logged
    /// hours.</param>
    /// <exception cref="InvalidOperationException">Thrown if the submitted hours worked overlap an existing submission.</exception>
    public void LogHours(DateTimeRange dateTimeRange)
    {
        if (_loggedHours.Any(dtr => dtr.Overlaps(dateTimeRange)))
            throw new InvalidOperationException("Submitted hours worked overlaps an existing submission.");

        _loggedHours.Add(dateTimeRange);
    }

    public void UpdateHourlyRate(decimal newHourlyRate)
    {
        if (newHourlyRate < 0)
            throw new ArgumentException("Hourly rate must be greater than or equal to zero.");
        HourlyRate = newHourlyRate;
    }

    #region Payroll Calculations

    /// <summary>
    /// Calculates the total payout for the specified pay period based on the employee's compensation type.
    /// </summary>
    /// <remarks>The payout calculation method is determined by the employee's compensation type, which can be
    /// either hourly or salary.</remarks>
    /// <param name="payPeriod">The date and time range representing the pay period for which the payout is calculated.</param>
    /// <returns>The total payout amount for the specified pay period, as a decimal value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the compensation type is not recognized.</exception>
    public decimal CalculatePayout(DateTimeRange payPeriod)
    {
        return CompensationType switch
        {
            CompensationType.Hourly => CalculateHourlyPayout(payPeriod),
            CompensationType.Salary => CalculateSalaryPayout(payPeriod),
            _ => throw new InvalidOperationException("Unknown compensation type.")
        };
    }

    private decimal CalculateHourlyPayout(DateTimeRange payPeriod)
    {
        // Calculate total hours worked within the pay period based on logged hours
        double totalHours = 0d;
        foreach (var logged in _loggedHours)
        {
            // Count hours that fall neatly within the pay period
            if (payPeriod.Contains(logged))
            {
                totalHours += logged.Duration.TotalHours;
            } else if (payPeriod.Overlaps(logged))
            {
                // If the logged hours overlap with the pay period, calculate the overlapping portion
                var validStart = payPeriod.Start.CompareTo(logged.Start) >= 0 ? payPeriod.Start : logged.Start;
                var validEnd = payPeriod.End.CompareTo(logged.End) >= 0 ? logged.End : payPeriod.End;
                totalHours += (validEnd - validStart).TotalHours;
            }
        }

        return ((decimal)Math.Min(totalHours, 40) * HourlyRate) + (decimal)Math.Max((totalHours - 40), 0) * HourlyRate * 1.5m;
    }

    private decimal CalculateSalaryPayout(DateTimeRange payPeriod)
    {
        // For salaried employees, we can calculate the payout based on the assumption of a
        // standard 40-hour workweek and the number of weeks in the pay period.
        // FORMULA: TotalDaysInPayPeriod / DaysInWeek * StandardHoursPerWeek * HourlyRate
        return (decimal)(payPeriod.Duration.TotalDays / 7 * 40) * HourlyRate;
    }

    #endregion

    public void Deactivate() => IsActive = false;
    public void Reactivate() => IsActive = true;
}
