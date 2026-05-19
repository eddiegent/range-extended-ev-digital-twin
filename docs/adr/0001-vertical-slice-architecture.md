# ADR 0001: Vertical Slice architecture

## Status

Accepted

## Decision

The API and application layers will be organized for Vertical Slice delivery so scenario control, telemetry, and history features can evolve with localized request/handler/read-model boundaries.

## Consequences

- Feature work stays localized.
- AI-assisted changes can target slice boundaries with less incidental coupling.
- Deep simulation modules can remain separate from request handling concerns.
