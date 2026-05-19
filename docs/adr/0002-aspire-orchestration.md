# ADR 0002: Aspire orchestration

## Status

Accepted

## Decision

.NET Aspire is the local orchestration layer for the API, frontend, PostgreSQL, RabbitMQ, and related developer-facing infrastructure.

## Consequences

- Service discovery and health visualization stay in the .NET developer workflow.
- The local topology is easier to run and explain.
- Container packaging remains complementary rather than replacing Aspire.
