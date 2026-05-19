namespace RangeExtendedEvDigitalTwin.Simulation.Abstractions;

public sealed record SimulationTickRequest(
    string ScenarioName,
    TimeSpan TickInterval,
    double SimulationSpeedMultiplier);
