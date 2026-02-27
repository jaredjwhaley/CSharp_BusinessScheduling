# Services

Domain Services contain business logic that does not naturally belong to a single entity or value object.  
They operate on entities, aggregates, or value objects to perform actions or enforce rules.

## Key Principles
- **Stateless**: Services should not maintain state; they operate on objects passed to them.
- **Domain-focused**: Encapsulate domain logic, not technical operations like database calls.
- **Reusability**: Services can be used by multiple aggregates or entities.

## Examples
- `SchedulingService` – Checks availability, enforces rules, and schedules appointments.
- `EmployeeManagementService` – Manages promotions, transfers, or work-hour calculations.
