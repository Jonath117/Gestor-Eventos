using QRCoder;

namespace Logistics.Application.Services;

public class QrGenerationService
{
    public string GenerateParticipantQrPayload(Guid participantId, Guid eventId)
    {
        // Dummy implementation for MVP. In reality, this would be a signed JWT.
        return $"{eventId}:{participantId}";
    }

    public byte[] GenerateQrImage(string payload)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
}
