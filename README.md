# Range-Extended EV Digital Twin

Portfolio-grade scaffold for a range-extended electric vehicle digital twin built as a .NET modular monolith with Vertical Slice delivery surfaces, Aspire orchestration, Dockerized local infrastructure, and a React operator console.

## Status

This repository currently contains **project scaffolding only**:

- .NET solution and project layout
- Aspire orchestration shell
- Docker and local infrastructure shell
- React + TypeScript + Tailwind frontend shell
- xUnit and Playwright test harnesses
- GitHub automation and architecture documentation scaffolding

No simulation features or business workflows are implemented yet.

## Planned architecture

- **Backend:** .NET 10 modular monolith
- **Structure:** Vertical Slice-friendly API/application layout with deep domain modules
- **Frontend:** React + TypeScript + Tailwind CSS
- **Realtime:** SignalR
- **Persistence:** PostgreSQL
- **Integration boundary:** RabbitMQ for selected external events
- **Auth infrastructure:** ASP.NET Core Identity + PostgreSQL
- **Orchestration:** .NET Aspire + Docker
- **Testing:** xUnit + Playwright

## Repository layout

```text
src/
  AppHost/
  ServiceDefaults/
  Api/
  Application/
  Simulation/
  Infrastructure/
  Contracts/
frontend/
tests/
docs/
deploy/
```

## Local development

Scaffold validation commands:

```powershell
dotnet build .\RangeExtendedEvDigitalTwin.slnx
dotnet test .\RangeExtendedEvDigitalTwin.slnx --no-build

Set-Location .\frontend
npm install
npm run lint
npm run build

Set-Location ..\tests\playwright
npm install
npx playwright install chromium
npm test
```

Intended local infrastructure flow:

1. Start Docker Desktop.
2. Run the Aspire AppHost from `src/AppHost` to boot the API, frontend, PostgreSQL, and RabbitMQ.
3. Use `deploy/docker/docker-compose.yml` for a containerized alternative of the same stack.

The scaffold assumes the following local infrastructure defaults:

- API URL: `http://127.0.0.1:8080` in Docker or the Aspire-assigned endpoint in AppHost
- PostgreSQL: `postgresql://postgres:postgres@127.0.0.1:5432/simulationdb`
- RabbitMQ management: `http://127.0.0.1:15672`

Authentication infrastructure is scaffolded with ASP.NET Core Identity backed by PostgreSQL, but no login or registration endpoints are implemented yet.

## Architecture records

- ADRs live in `docs/adr/`
- architecture notes live in `docs/architecture/`
- diagrams live in `docs/diagrams/`
