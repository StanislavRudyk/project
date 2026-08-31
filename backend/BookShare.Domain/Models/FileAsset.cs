namespace BookShare.Domain.Models;

public sealed class FileAsset
{
    public string Hash { get; set; } = string.Empty;

    public Guid EditionIdentifier { get; set; }
    public Edition Edition { get; set; } = null!;

    public string? ContentHash { get; set; }

    public long SizeInBytes { get; set; }
    public string MimeType { get; set; } = "application/pdf";
    public bool IsDmcaBanned { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}