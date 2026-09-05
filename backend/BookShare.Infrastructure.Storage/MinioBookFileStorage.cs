using BookShare.Domain.Abstractions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace BookShare.Infrastructure.Storage;

public sealed class MinioBookFileStorage : IBookFileStorage
{
    private readonly IMinioClient _minio;
    private readonly IMinioClient _publicMinio;
    private readonly MinioOptions _options;

    public MinioBookFileStorage(
        IMinioClient minio,
        IOptions<MinioOptions> options)
    {
        _minio = minio;
        _options = options.Value;

        _publicMinio = new MinioClient()
            .WithEndpoint(_options.PublicEndpoint)
            .WithCredentials(
                _options.AccessKey,
                _options.SecretKey)
            .WithSSL(_options.UseSsl)
            .Build();
    }

    public async Task<string> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(fileName);

        var fileKey = $"books/{Guid.NewGuid()}{extension}";

        var args = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(fileKey)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);

        await _minio.PutObjectAsync(
            args,
            cancellationToken);

        return fileKey;
    }

    public async Task<string> GetPresignedUrlAsync(
        string fileKey,
        int expirySeconds = 3600,
        CancellationToken cancellationToken = default)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(fileKey)
            .WithExpiry(expirySeconds);

        return await _publicMinio.PresignedGetObjectAsync(args);
    }

    public async Task<string> UploadCoverAsync(
        Stream stream,
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var fileKey = $"covers/{bookId}.jpg";

        var args = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(fileKey)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType("image/jpeg");

        await _minio.PutObjectAsync(
            args,
            cancellationToken);

        return fileKey;
    }

    public async Task<Stream> GetAsync(
        string fileKey,
        CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(fileKey)
            .WithCallbackStream(async responseStream =>
            {
                await responseStream.CopyToAsync(
                    stream,
                    cancellationToken);
            });

        await _minio.GetObjectAsync(
            args,
            cancellationToken);

        stream.Position = 0;

        return stream;
    }
}