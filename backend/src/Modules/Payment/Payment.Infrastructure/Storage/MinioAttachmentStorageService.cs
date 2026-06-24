using Amazon.S3;
using Amazon.S3.Model;
using Payment.Application.Abstractions;

namespace Payment.Infrastructure.Storage;

public class MinioAttachmentStorageService(IAmazonS3 s3Client) : IAttachmentStorageService
{
    private const string BucketName = "gestor-eventos";
    private const string ReceiptsFolder = "receipts";

    public async Task<string> SaveReceiptAsync(Guid applicationId, string base64Content)
    {
        if (string.IsNullOrWhiteSpace(base64Content))
            return string.Empty;

        string payload = base64Content.Contains(',')
            ? base64Content[(base64Content.IndexOf(',') + 1)..]
            : base64Content;

        byte[] bytes = Convert.FromBase64String(payload);

        string fileName = $"{applicationId}.png";
        string key = $"{ReceiptsFolder}/{fileName}";

        using var stream = new MemoryStream(bytes);
        var putRequest = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = key,
            InputStream = stream,
            ContentType = "image/png"
        };

        await s3Client.PutObjectAsync(putRequest);

        return $"http://localhost:9000/{BucketName}/{key}";
    }
}
