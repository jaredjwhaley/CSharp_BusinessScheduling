# Events

Domain Events represent something that happened in the domain that other parts of the system may be interested in.  
They communicate changes or significant occurrences within the domain.

## Key Principles
- **Immutable**: Once created, the event cannot change.
- **Past Tense Naming**: Name events in the past tense, e.g., `AppointmentScheduled`.
- **Decoupling**: Events allow different parts of the system to react without direct coupling.

## Examples
- `EmployeeHired` – Fired when a new employee is added.
- `AppointmentCancelled` – Fired when an appointment is cancelled.
- `ScheduleConflictDetected` – Fired when overlapping schedules are detected.
