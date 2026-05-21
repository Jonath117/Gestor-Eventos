using FluentAssertions;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using NSubstitute;

using Registration.Application.Features.Registration.RequestOtp;

using Xunit;

namespace Registration.Application.UnitTests.Features.Registration.RequestOtp;

public class RequestOtpHandlerTests
{
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly ILogger<RequestOtpHandler> _logger = Substitute.For<ILogger<RequestOtpHandler>>();
    private readonly RequestOtpHandler _handler;

    public RequestOtpHandlerTests()
    {
        _handler = new RequestOtpHandler(_cache, _logger);
    }

    [Fact]
    public async Task Handle_ShouldGenerateOtpAndStoreInCache()
    {
        // Arrange
        var command = new RequestOtpCommand(Guid.NewGuid(), "test@example.com", "John Doe");
        var cacheKey = $"OTP_{command.EventId}_{command.Email}";

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _cache.TryGetValue(cacheKey, out string? otp).Should().BeTrue();
        otp.Should().HaveLength(6);
    }
}