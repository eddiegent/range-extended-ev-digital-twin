namespace RangeExtendedEvDigitalTwin.Infrastructure.Configuration;

public sealed class BootstrapOperatorOptions
{
    public const string SectionName = "BootstrapOperator";

    public bool Enabled { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
