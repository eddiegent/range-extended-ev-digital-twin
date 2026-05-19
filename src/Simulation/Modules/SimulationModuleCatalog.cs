namespace RangeExtendedEvDigitalTwin.Simulation.Modules;

public static class SimulationModuleCatalog
{
    public static IReadOnlyList<string> All { get; } =
    [
        "Vehicle",
        "EnergyManagement",
        "BrakingAndRecovery",
        "FaultHandling",
        "ProjectionPipeline"
    ];
}
