namespace BookShare.Domain.Models; 

public sealed class Work
{
    public Guid Identifier { get; set; }

    public string OriginalTitle { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public DateOnly? DateOfFirstPublication { get; set; }

    public string? Genres { get; set; } 

    public double? GlobalRanking { get; set; }

    public ICollection<Edition> Editions { get; set; } = new List<Edition>();
    
}