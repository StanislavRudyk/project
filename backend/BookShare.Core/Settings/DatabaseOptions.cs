namespace BookShare.Core.Settings;

public sealed class DatabaseOptions
{
    public const string SectionName = "DatabaseSettings";
    public string ConnectionString { get; set; } = string.Empty;
}