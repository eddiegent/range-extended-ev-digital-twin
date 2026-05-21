using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RangeExtendedEvDigitalTwin.Infrastructure.Configuration;

namespace RangeExtendedEvDigitalTwin.Infrastructure.Persistence;

public sealed class SimulationDbContextFactory : IDesignTimeDbContextFactory<SimulationDbContext>
{
    public SimulationDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__simulationdb")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings:simulationdb")
            ?? "Host=127.0.0.1;Port=5432;Database=simulationdb;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<SimulationDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            npgsql =>
                npgsql.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    DatabaseSchemas.Authentication));

        return new SimulationDbContext(optionsBuilder.Options);
    }
}
