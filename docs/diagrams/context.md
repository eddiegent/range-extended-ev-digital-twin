# Context diagram

```mermaid
flowchart LR
    Operator[Operator] --> Frontend[React operator console]
    Frontend --> Api[.NET API + SignalR]
    Api --> Simulation[Simulation modules]
    Api --> Identity[ASP.NET Core Identity]
    Identity --> Postgres[(PostgreSQL)]
    Api --> RabbitMQ[(RabbitMQ)]
```
