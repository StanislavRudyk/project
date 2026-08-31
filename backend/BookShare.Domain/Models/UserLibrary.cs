namespace BookShare.Domain.Models; 

public sealed class UserLibrary{

    public Guid Identifier { get; set; }

    public Guid FileAssetIdentifier { get; set; }
    public FileAsset FileAsset { get; set; } = null!;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    

}
