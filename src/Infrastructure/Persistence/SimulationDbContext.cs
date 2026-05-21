using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RangeExtendedEvDigitalTwin.Infrastructure.Authentication;
using RangeExtendedEvDigitalTwin.Infrastructure.Configuration;

namespace RangeExtendedEvDigitalTwin.Infrastructure.Persistence;

public sealed class SimulationDbContext(DbContextOptions<SimulationDbContext> options)
    : IdentityDbContext<OperatorUser, OperatorRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(DatabaseSchemas.Authentication);

        base.OnModelCreating(builder);
    }
}
