# Aggregates

Aggregates are clusters of domain objects that are treated as a single unit for data changes.  
Each aggregate has a single root entity (Aggregate Root) that controls access and consistency of the other entities within the aggregate.

## Key Principles
- **Aggregate Root**: The main entity through which all access occurs. Only it can be referenced externally.
- **Encapsulation**: Internal entities and value objects are not directly manipulated from outside the aggregate.
- **Consistency**: Ensures business rules are enforced at the aggregate boundary.
- **Transactional Boundary**: Changes within an aggregate should be committed as a single transaction.

## Examples
- `EmployeeScheduleAggregate` could contain an `Employee` entity, a `Schedule` entity, and `TimeRange` value objects.
- `AppointmentAggregate` could contain the appointment, associated employee, and location details.

Aggregates help ensure data consistency and enforce rules at a natural boundary in your domain.
