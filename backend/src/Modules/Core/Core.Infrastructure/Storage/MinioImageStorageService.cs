using Amazon.S3;
using Amazon.S3.Model;
using Core.Application.Abstractions;

namespace Core.Infrastructure.Storage;

public class MinioImageStorageService(IAmazonS3 s3Client, StorageOptions storageOptions) : IImageStorageService
{
    public async Task<string?> SaveImageAsync(
        string? base64Content,
        string folder,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(base64Content))
        {
            return null;
        }

        string payload = base64Content.Contains(',')
            ? base64Content[(base64Content.IndexOf(',') + 1)..]
            : base64Content;

        byte[] bytes = Convert.FromBase64String(payload);

        string fileName = $"{Guid.NewGuid()}.png";
        string key = $"{folder}/{fileName}";

        using var stream = new MemoryStream(bytes);
        var putRequest = new PutObjectRequest
        {
            BucketName = storageOptions.BucketName,
            Key = key,
            InputStream = stream,
            ContentType = "image/png"
        };

        await s3Client.PutObjectAsync(putRequest, cancellationToken);

        return $"{storageOptions.PublicBaseUrl}/{storageOptions.BucketName}/{key}";
    }
}
