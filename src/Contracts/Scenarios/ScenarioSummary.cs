namespace RangeExtendedEvDigitalTwin.Contracts.Scenarios;

public sealed record ScenarioSummary(
    string Name,
    string DisplayName,
    string Description,
    bool SupportsFaultInjection);
