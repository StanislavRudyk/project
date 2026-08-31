namespace BookShare.Domain.Models;

public sealed class Edition
{
    public Guid Identifier { get; set; }

    public Guid WorkIdentifier { get; set; }
    public Work Work { get; set; } = null!;

    public string Language { get; set; } = string.Empty;
    public string? Translator { get; set; }
    public int? PublicationYear { get; set; }
    public string? Publisher { get; set; }
    public string? CoverUrl { get; set; }

    public ICollection<FileAsset> FileAssets { get; set; } = new List<FileAsset>();
}