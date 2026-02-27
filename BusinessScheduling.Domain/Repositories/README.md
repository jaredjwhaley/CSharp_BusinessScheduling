# Repositories

Repositories abstract access to data sources and provide methods to retrieve, store, or update aggregates.  
They hide the underlying storage mechanism (SQL, in-memory, file, etc.) from the domain.

## Key Principles
- **Interface-based**: Define the repository contract in the domain layer.
- **Persistence Agnostic**: The domain logic should not depend on the storage technology.
- **Aggregate Operations**: Repositories operate on aggregate roots, not individual entities inside them.

## Examples
- `IEmployeeRepository` – Defines methods to fetch employees, add new employees, or update existing ones.
- `IAppointmentRepository` – Defines methods to fetch, schedule, or cancel appointments.
