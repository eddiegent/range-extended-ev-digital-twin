using RangeExtendedEvDigitalTwin.Contracts.Projections;
using RangeExtendedEvDigitalTwin.Simulation.Modules;

namespace RangeExtendedEvDigitalTwin.Architecture.Tests;

public sealed class SolutionArchitectureTests
{
    [Fact]
    public void Core_scaffold_contracts_and_modules_are_present()
    {
        Assert.NotEmpty(SimulationModuleCatalog.All);
        Assert.Equal("VehicleStateProjection", typeof(VehicleStateProjection).Name);
    }
}
