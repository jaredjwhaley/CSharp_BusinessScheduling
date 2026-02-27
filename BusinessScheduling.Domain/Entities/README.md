# Entities

Entities are objects that have a unique identity and a lifecycle.  
They are mutable (state can change over time) and are compared based on their identity rather than their attributes.

## Key Principles
- **Identity**: Each entity has a unique identifier (e.g., EmployeeId, AppointmentId).
- **Lifecycle**: Entities are created, updated, and eventually deleted.
- **Equality**: Two entities are equal if they share the same identity, even if other properties differ.
- **Behavior**: Encapsulate domain logic and operations relevant to that entity.

## Examples
- `Employee` – Represents an individual working in the system.
- `Appointment` – Represents a scheduled appointment with properties like date/time, assigned employee, and location.
