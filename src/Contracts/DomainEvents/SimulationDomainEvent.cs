namespace RangeExtendedEvDigitalTwin.Contracts.DomainEvents;

public interface ISimulationDomainEvent
{
    DomainEventMetadata Metadata { get; }
}
