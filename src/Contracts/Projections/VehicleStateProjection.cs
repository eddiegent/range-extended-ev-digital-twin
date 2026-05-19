namespace RangeExtendedEvDigitalTwin.Contracts.Projections;

public sealed record VehicleStateProjection(
    string ScenarioName,
    string SimulationStatus,
    double BatteryStateOfChargePercent,
    bool GeneratorActive,
    DateTimeOffset AsOf);
