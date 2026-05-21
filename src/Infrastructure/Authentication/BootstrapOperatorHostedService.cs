using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RangeExtendedEvDigitalTwin.Infrastructure.Configuration;
using RangeExtendedEvDigitalTwin.Infrastructure.Persistence;

namespace RangeExtendedEvDigitalTwin.Infrastructure.Authentication;

public sealed class BootstrapOperatorHostedService(
    IServiceScopeFactory scopeFactory,
    IOptions<BootstrapOperatorOptions> bootstrapOptions,
    ILogger<BootstrapOperatorHostedService> logger) : IHostedService
{
    private const string OperatorRoleName = "operator";

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var options = bootstrapOptions.Value;

        if (!options.Enabled)
        {
            return;
        }

        ValidateOptions(options);

        using var scope = scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var dbContext = serviceProvider.GetRequiredService<SimulationDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<OperatorRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<OperatorUser>>();

        await EnsureIdentityStoreReadyAsync(dbContext, cancellationToken);

        if (!await roleManager.RoleExistsAsync(OperatorRoleName))
        {
            var createRoleResult = await roleManager.CreateAsync(new OperatorRole
            {
                Name = OperatorRoleName,
                NormalizedName = OperatorRoleName.ToUpperInvariant()
            });

            if (!createRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create bootstrap operator role: {string.Join(", ", createRoleResult.Errors.Select(error => error.Description))}");
            }
        }

        var user = await userManager.FindByEmailAsync(options.Email);

        if (user is null)
        {
            user = new OperatorUser
            {
                UserName = options.UserName,
                Email = options.Email,
                EmailConfirmed = true
            };

            var createUserResult = await userManager.CreateAsync(user, options.Password);

            if (!createUserResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create bootstrap operator user: {string.Join(", ", createUserResult.Errors.Select(error => error.Description))}");
            }

            logger.LogInformation("Created bootstrap operator account for {Email}.", options.Email);
        }

        if (!await userManager.IsInRoleAsync(user, OperatorRoleName))
        {
            var addToRoleResult = await userManager.AddToRoleAsync(user, OperatorRoleName);

            if (!addToRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to assign bootstrap operator role: {string.Join(", ", addToRoleResult.Errors.Select(error => error.Description))}");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task EnsureIdentityStoreReadyAsync(
        SimulationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var providerName = dbContext.Database.ProviderName;

        if (string.Equals(providerName, "Npgsql.EntityFrameworkCore.PostgreSQL", StringComparison.Ordinal))
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            return;
        }

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }

    private static void ValidateOptions(BootstrapOperatorOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.UserName)
            || string.IsNullOrWhiteSpace(options.Email)
            || string.IsNullOrWhiteSpace(options.Password))
        {
            throw new InvalidOperationException(
                "Bootstrap operator configuration requires UserName, Email, and Password when enabled.");
        }
    }
}
