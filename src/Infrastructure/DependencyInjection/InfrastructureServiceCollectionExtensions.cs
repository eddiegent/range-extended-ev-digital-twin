using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RangeExtendedEvDigitalTwin.Infrastructure.Authentication;
using RangeExtendedEvDigitalTwin.Infrastructure.Configuration;
using RangeExtendedEvDigitalTwin.Infrastructure.Persistence;

namespace RangeExtendedEvDigitalTwin.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<InfrastructureOptions>(
            configuration.GetSection(InfrastructureOptions.SectionName));

        var connectionString = configuration.GetConnectionString("simulationdb")
            ?? throw new InvalidOperationException(
                "Connection string 'simulationdb' is required for PostgreSQL-backed infrastructure.");

        services.AddDbContext<SimulationDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsql =>
                    npgsql.MigrationsHistoryTable(
                        "__EFMigrationsHistory",
                        DatabaseSchemas.Authentication)));

        services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();

        services.AddAuthorization();

        services.AddIdentityCore<OperatorUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<OperatorRole>()
            .AddEntityFrameworkStores<SimulationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddSingleton(new InfrastructureAssemblyMarker());

        return services;
    }
}
