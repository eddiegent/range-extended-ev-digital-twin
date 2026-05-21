# ADR 0004: .NET authentication with PostgreSQL

## Status

Accepted

## Decision

Authentication infrastructure lives inside the .NET application using ASP.NET Core Identity backed by PostgreSQL. The application owns user tables, auth flows, and database migrations instead of delegating authentication to an external platform.

## Consequences

- The entire local stack can stay inside Aspire and Docker without a separate Supabase dependency.
- PostgreSQL becomes the single backing store for both authentication and simulation-related persistence.
- Login, registration, token issuance, and authorization policies can be implemented later as application features on top of this shared infrastructure.
