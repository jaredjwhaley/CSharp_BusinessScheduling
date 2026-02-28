# BusinessScheduling.Domain

This project contains the **domain layer** for the Business Scheduling system.

It is intentionally framework-agnostic and focuses exclusively on modeling
business concepts, rules, and constraints related to scheduling and compensation.
No infrastructure, persistence, UI, or application orchestration logic exists here.

## Responsibilities of the Domain Layer

- Model core business concepts and their relationships
- Enforce business rules and invariants
- Prevent invalid states through explicit modeling
- Remain independent of external technologies and frameworks

## What Belongs Here

- Aggregates that enforce consistency boundaries
- Entities with identity and lifecycle
- Value Objects that are immutable and compared by value
- Domain services for cross-aggregate operations
- Domain events representing significant occurrences
- Repository interfaces defining persistence contracts

## What Does NOT Belong Here

- Database access or ORM configuration
- Web APIs, controllers, or UI concerns
- Application workflows or coordination logic
- File storage, messaging, or infrastructure details

## Design Principles

- Invalid states should be difficult or impossible to represent
- Business rules live close to the data they govern
- Aggregates protect their own invariants
- External systems interact with the domain through clear boundaries

## Folder Structure

- `Aggregates/` – Consistency boundaries with a single aggregate root
- `Entities/` – Objects with identity and lifecycle
- `ValueObjects/` – Immutable objects compared by value
- `Repositories/` – Interfaces for retrieving and persisting aggregates
- `Services/` – Domain operations not naturally owned by a single entity
- `Events/` – Represent significant domain occurrences

## Status

The domain is under active development. Current focus areas include:
- Scheduling primitives
- Compensation rules
- Time-based constraints
- Clear documentation of domain intent
