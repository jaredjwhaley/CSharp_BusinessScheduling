# BusinessScheduling.Tests

This project contains automated tests for the BusinessScheduling domain model.

The tests are written using **xUnit** and follow Domain-Driven Design (DDD)
testing principles, focusing on correctness, clarity, and isolation.

## Goals

- Verify domain invariants and business rules
- Ensure value objects implement correct value semantics
- Validate entity and aggregate behavior
- Provide fast, deterministic feedback during development

## Design Principles

- One test class per domain type
- Tests are isolated and stateless
- Domain logic is tested independently of infrastructure concerns
- Shared setup uses xUnit fixtures only when necessary
