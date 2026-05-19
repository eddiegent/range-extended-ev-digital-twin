namespace RangeExtendedEvDigitalTwin.Contracts.IntegrationEvents;

public interface ISimulationIntegrationEvent
{
    Guid EventId { get; }
    DateTimeOffset PublishedAt { get; }
}
