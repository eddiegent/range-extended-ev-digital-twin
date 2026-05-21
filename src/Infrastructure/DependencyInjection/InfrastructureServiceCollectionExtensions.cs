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
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? configureDbContext = null)
    {
        services.Configure<InfrastructureOptions>(
            configuration.GetSection(InfrastructureOptions.SectionName));
        services.Configure<BootstrapOperatorOptions>(
            configuration.GetSection(BootstrapOperatorOptions.SectionName));

        services.AddDbContext<SimulationDbContext>(
            options =>
            {
                if (configureDbContext is not null)
                {
                    configureDbContext(options);
                    return;
                }

                var connectionString = configuration.GetConnectionString("simulationdb")
                    ?? throw new InvalidOperationException(
                        "Connection string 'simulationdb' is required for PostgreSQL-backed infrastructure.");

                options.UseNpgsql(
                    connectionString,
                    npgsql =>
                        npgsql.MigrationsHistoryTable(
                            "__EFMigrationsHistory",
                            DatabaseSchemas.Authentication));
            });

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

        services.AddHostedService<BootstrapOperatorHostedService>();
        services.AddSingleton(new InfrastructureAssemblyMarker());

        return services;
    }
}
