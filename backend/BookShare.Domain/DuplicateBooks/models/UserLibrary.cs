namespace BookShare.Domain.Models; 

public sealed class UserLibrary
{
    public Guid Identifier { get; set; }

    public string FileAssetHash { get; set; } = string.Empty;
    public FileAsset FileAsset { get; set; } = null!;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}