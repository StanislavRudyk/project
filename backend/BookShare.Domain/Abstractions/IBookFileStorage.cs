namespace BookShare.Domain.Abstractions;

public interface IBookFileStorage
{
    Task<string> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);

    Task<string> UploadCoverAsync(
        Stream stream,
        Guid bookId,
        CancellationToken cancellationToken = default);

    Task<string> GetPresignedUrlAsync(
        string fileKey,
        int expirySeconds = 3600,
        CancellationToken cancellationToken = default);

    Task<Stream> GetAsync(
        string fileKey,
        CancellationToken cancellationToken = default);
}