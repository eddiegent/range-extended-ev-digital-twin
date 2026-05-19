namespace RangeExtendedEvDigitalTwin.Simulation.Abstractions;

public interface IVehicleSimulationEngine
{
    ValueTask AdvanceAsync(SimulationTickRequest request, CancellationToken cancellationToken);
}
