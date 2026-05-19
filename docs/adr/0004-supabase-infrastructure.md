# ADR 0004: Supabase as infrastructure

## Status

Accepted

## Decision

Supabase is treated as infrastructure for authentication and hosted PostgreSQL-compatible workflows rather than as the application runtime.

## Consequences

- Auth remains easy to demo without moving domain behavior out of .NET.
- Local infrastructure scaffolding can evolve independently of domain logic.
