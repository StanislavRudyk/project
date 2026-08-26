namespace BookShare.Domain.Abstractions;

public interface IBookFileStorage
{
    Task<string> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);
}