# Architecture overview

This repository is scaffolded as a **modular monolith** with explicit delivery and domain boundaries:

- `src/Api` hosts HTTP and SignalR entrypoints.
- `src/Application` hosts application composition and slice-facing orchestration.
- `src/Simulation` hosts the future deep modules for vehicle behavior and time-stepped simulation.
- `src/Infrastructure` hosts persistence, messaging, auth, and integration wiring.
- `src/Contracts` hosts shared contracts for domain events, projections, scenarios, and realtime messages.
- `frontend` hosts the operator console shell.
- `tests` hosts backend and browser test harnesses.

Stage 1 focuses on scaffolding, orchestration, and development workflow. Domain behavior is intentionally not implemented yet.
