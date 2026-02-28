namespace BusinessScheduling.Domain.ValueObjects;

/// <summary>
/// Specifies the types of time off that an employee may take.
/// </summary>
/// <remarks>Use this enumeration to categorize employee absences for payroll, benefits, and human resources
/// management. The values represent common leave types such as paid time off, sick leave, and other special
/// circumstances. This enumeration can be extended to support additional leave types as required by organizational
/// policies or local regulations.</remarks>
public enum TimeOffType
{
    PaidTimeOff,
    SickLeave,
    Bereavement,
    JuryDuty,
    UnpaidLeave,
    NoShow,
    Other
}