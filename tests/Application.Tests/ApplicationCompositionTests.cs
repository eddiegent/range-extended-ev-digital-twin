using RangeExtendedEvDigitalTwin.Application.DependencyInjection;

namespace RangeExtendedEvDigitalTwin.Application.Tests;

public sealed class ApplicationCompositionTests
{
    [Fact]
    public void Application_assembly_marker_is_present()
    {
        Assert.Equal("ApplicationAssemblyMarker", typeof(ApplicationAssemblyMarker).Name);
    }
}
