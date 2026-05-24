using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RangeExtendedEvDigitalTwin.Infrastructure.Configuration;
using RangeExtendedEvDigitalTwin.Infrastructure.Authentication;
using RangeExtendedEvDigitalTwin.Infrastructure.DependencyInjection;
using RangeExtendedEvDigitalTwin.Infrastructure.Persistence;

namespace RangeExtendedEvDigitalTwin.Infrastructure.Tests;

public sealed class InfrastructureCompositionTests
{
    [Fact]
    public void Infrastructure_registers_identity_and_postgres_backed_services()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>(
                    "ConnectionStrings:simulationdb",
                    "Host=localhost;Port=5432;Database=simulationdb;Username=postgres;Password=postgres")
            ])
            .Build();

        var services = new ServiceCollection();

        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();

        Assert.Equal("InfrastructureAssemblyMarker", typeof(InfrastructureAssemblyMarker).Name);
        Assert.Equal("auth", new InfrastructureOptions().AuthenticationSchema);
        Assert.NotNull(provider.GetService<SimulationDbContext>());
        Assert.NotNull(provider.GetService<IUserStore<OperatorUser>>());

        var identityOptions = provider.GetRequiredService<IOptions<IdentityOptions>>().Value;
        Assert.True(identityOptions.Lockout.AllowedForNewUsers);
        Assert.Equal(5, identityOptions.Lockout.MaxFailedAccessAttempts);
        Assert.Equal(TimeSpan.FromMinutes(15), identityOptions.Lockout.DefaultLockoutTimeSpan);

        var applicationCookieOptions = provider
            .GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>()
            .Get(IdentityConstants.ApplicationScheme);
        Assert.True(applicationCookieOptions.Cookie.HttpOnly);
        Assert.Equal(SameSiteMode.Strict, applicationCookieOptions.Cookie.SameSite);
        Assert.Equal(CookieSecurePolicy.SameAsRequest, applicationCookieOptions.Cookie.SecurePolicy);
        Assert.Equal(TimeSpan.FromHours(8), applicationCookieOptions.ExpireTimeSpan);
        Assert.True(applicationCookieOptions.SlidingExpiration);
    }
}
