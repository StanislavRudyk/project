namespace BookShare.Infrastructure.Storage;

public sealed class MinioOptions
{
    public const string SectionName = "Minio";
    public string Endpoint { get; set; } = default!;
    public string PublicEndpoint { get; set; } = default!;
    public string AccessKey { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public string BucketName { get; set; } = default!;
    public bool UseSsl { get; set; }
}