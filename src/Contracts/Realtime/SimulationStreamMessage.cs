using RangeExtendedEvDigitalTwin.Contracts.Projections;

namespace RangeExtendedEvDigitalTwin.Contracts.Realtime;

public sealed record SimulationStreamMessage(
    VehicleStateProjection CurrentState,
    IReadOnlyList<string> ActiveSubsystems);
