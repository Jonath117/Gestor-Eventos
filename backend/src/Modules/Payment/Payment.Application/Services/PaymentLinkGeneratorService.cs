namespace Payment.Application.Services;

public class PaymentLinkGeneratorService
{
    public Task<string> GenerateLinkAsync(Guid applicationId)
    {
        // Dummy implementation simulating a payment gateway link generation (e.g. Stripe/MercadoPago)
        var dummyLink = $"https://checkout.campeando.dummy/pay/{applicationId}?token={Guid.NewGuid()}";
        Console.WriteLine($"[Payment Simulation] Generated payment link for application {applicationId}: {dummyLink}");
        return Task.FromResult(dummyLink);
    }
}
