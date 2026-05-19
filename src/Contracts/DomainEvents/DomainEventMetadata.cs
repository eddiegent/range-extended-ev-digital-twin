namespace RangeExtendedEvDigitalTwin.Contracts.DomainEvents;

public sealed record DomainEventMetadata(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string StreamName,
    string EventType);
