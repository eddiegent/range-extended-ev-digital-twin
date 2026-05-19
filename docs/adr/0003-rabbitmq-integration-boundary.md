# ADR 0003: RabbitMQ integration boundary

## Status

Accepted

## Decision

RabbitMQ is reserved for selected external integration events. Internal domain communication remains in-process inside the modular monolith.

## Consequences

- The core domain model is not distorted by queue semantics.
- External event consumption remains demonstrable for interviews and extensions.
