using RangeExtendedEvDigitalTwin.Simulation.Modules;

namespace RangeExtendedEvDigitalTwin.Simulation.Tests;

public sealed class SimulationArchitectureTests
{
    [Fact]
    public void Simulation_catalog_lists_expected_stage_one_modules()
    {
        Assert.Contains("EnergyManagement", SimulationModuleCatalog.All);
    }
}
