# Business Scheduling Domain

This repository contains a domain model for a generalized business scheduling system.
It is intentionally framework-agnostic and focuses on enforcing core business rules
related to scheduling, availability, and operating constraints.

## Goals

- Model real-world scheduling concepts in a way that prevents invalid states
- Keep business rules close to the data they govern
- Remain independent of storage, UI, and framework concerns
- Support reuse across multiple business domains (e.g., retail, healthcare, services)- Demonstrate clean DDD architecture
- Showcase C#/.NET proficiency
- Provide a foundation for REST API integration or document/OCR workflows
- Build maintainable, testable, and scalable domain models

## Non-Goals

- User interfaces
- Database access or ORM configuration
- Web frameworks or APIs
- Application workflows or orchestration logic

## Core Concepts

This domain models concepts such as:
- Employees
- Availability and operating hours
- Schedules and time constraints
- Appointments and bookings

Business rules are enforced within the domain model itself rather than
being delegated to external services.

## Design Principles

- Invalid states should be difficult or impossible to represent
- Domain objects own their own invariants
- All external concerns are handled by higher layers

## Folder Structure
- `Aggregates/` – Clusters of entities with a single root, enforcing consistency
- `Entities/` – Objects with unique identity and lifecycle
- `ValueObjects/` – Immutable objects compared by value
- `Repositories/` – Abstract storage and retrieval of aggregates
- `Services/` – Domain-specific operations that do not belong to a single entity
- `Events/` – Represent significant occurrences within the domain

## Status

This project is under active development. The initial focus is on
core scheduling primitives and rules.
