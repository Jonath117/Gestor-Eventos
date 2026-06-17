using FluentAssertions;

using Microsoft.Extensions.Caching.Memory;

using NSubstitute;

using Registration.Application.Features.Registration.VerifyOtp;

using Xunit;

namespace Registration.Application.UnitTests.Features.Registration.VerifyOtp;

public class VerifyOtpHandlerTests
{
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly VerifyOtpHandler _handler;

    public VerifyOtpHandlerTests()
    {
        _handler = new VerifyOtpHandler(_cache);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenOtpIsValid()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var email = "test@example.com";
        var otp = "123456";
        var command = new VerifyOtpCommand(eventId, email, otp);
        var cacheKey = $"OTP_{eventId}_{email}";
        var verifiedKey = $"Verified_{eventId}_{email}";

        _cache.Set(cacheKey, otp);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _cache.TryGetValue(cacheKey, out _).Should().BeFalse();
        _cache.TryGetValue(verifiedKey, out bool isVerified).Should().BeTrue();
        isVerified.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenOtpIsInvalid()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var email = "test@example.com";
        var command = new VerifyOtpCommand(eventId, email, "wrong");
        var cacheKey = $"OTP_{eventId}_{email}";

        _cache.Set(cacheKey, "123456");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _cache.TryGetValue(cacheKey, out _).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenOtpIsExpired()
    {
        // Arrange
        var command = new VerifyOtpCommand(Guid.NewGuid(), "test@example.com", "123456");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}