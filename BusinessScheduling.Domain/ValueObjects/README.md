# Value Objects

Value Objects are immutable objects that describe attributes or properties.  
They do not have a unique identity; equality is based on their values.

## Key Principles
- **Immutability**: Once created, the state of a value object cannot be changed.
- **Equality by Value**: Two value objects are equal if all their properties are equal.
- **Self-contained Logic**: Can enforce rules internally (e.g., `TimeRange` cannot have an end before a start).

## Examples
- `TimeRange` – Represents a start and end time for scheduling.
- `Address` – Represents location details.
- `Money` – Represents an amount and currency.
