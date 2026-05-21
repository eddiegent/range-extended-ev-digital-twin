namespace RangeExtendedEvDigitalTwin.Infrastructure.Configuration;

public sealed class InfrastructureOptions
{
    public const string SectionName = "Infrastructure";

    public string AuthenticationSchema { get; init; } = DatabaseSchemas.Authentication;

    public string EventStoreSchema { get; init; } = DatabaseSchemas.EventStore;

    public string ProjectionSchema { get; init; } = DatabaseSchemas.Projection;
}
