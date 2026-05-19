namespace RangeExtendedEvDigitalTwin.Infrastructure.Configuration;

public sealed class InfrastructureOptions
{
    public const string SectionName = "Infrastructure";

    public string EventStoreSchema { get; init; } = "simulation";

    public string ProjectionSchema { get; init; } = "projection";
}
