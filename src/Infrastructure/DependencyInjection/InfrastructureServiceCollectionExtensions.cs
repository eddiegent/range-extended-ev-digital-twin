using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RangeExtendedEvDigitalTwin.Infrastructure.Configuration;

namespace RangeExtendedEvDigitalTwin.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<InfrastructureOptions>(
            configuration.GetSection(InfrastructureOptions.SectionName));

        services.AddSingleton(new InfrastructureAssemblyMarker());

        return services;
    }
}
