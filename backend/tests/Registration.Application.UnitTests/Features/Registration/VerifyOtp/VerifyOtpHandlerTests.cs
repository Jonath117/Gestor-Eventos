using System;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Registration.Application.Features.Registration.VerifyOtp;
using Registration.Domain.Entities;
using Registration.Infrastructure.Database;

using Xunit;

namespace Registration.Application.UnitTests.Features.Registration.VerifyOtp;

public class VerifyOtpHandlerTests : IDisposable
{
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly RegistrationDbContext _dbContext;
    private readonly VerifyOtpHandler _handler;

    public VerifyOtpHandlerTests()
    {
        var options = new DbContextOptionsBuilder<RegistrationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new RegistrationDbContext(options);
        _handler = new VerifyOtpHandler(_dbContext, _cache);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenOtpIsValid()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var email = "test@example.com";
        var otp = "123456";
        var command = new VerifyOtpCommand(eventId, email, otp);
        var verifiedKey = $"Verified_{eventId}_{email}";

        var otpRequest = new OtpRequest
        {
            Id = Guid.NewGuid(),
            UserId = email,
            TenantId = eventId.ToString(),
            Code = otp,
            Status = "procesado",
            CreatedAt = DateTime.UtcNow
        };
        await _dbContext.OtpRequests!.AddAsync(otpRequest);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Verify database state updated
        var updatedRecord = await _dbContext.OtpRequests.FindAsync(otpRequest.Id);
        updatedRecord.Should().NotBeNull();
        updatedRecord!.Status.Should().Be("verificado");

        // Verify cache state updated
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

        var otpRequest = new OtpRequest
        {
            Id = Guid.NewGuid(),
            UserId = email,
            TenantId = eventId.ToString(),
            Code = "123456",
            Status = "procesado",
            CreatedAt = DateTime.UtcNow
        };
        await _dbContext.OtpRequests!.AddAsync(otpRequest);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify database state not changed to verificado
        var updatedRecord = await _dbContext.OtpRequests.FindAsync(otpRequest.Id);
        updatedRecord.Should().NotBeNull();
        updatedRecord!.Status.Should().Be("procesado");
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenOtpIsExpired()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var email = "test@example.com";
        var otp = "123456";
        var command = new VerifyOtpCommand(eventId, email, otp);

        var otpRequest = new OtpRequest
        {
            Id = Guid.NewGuid(),
            UserId = email,
            TenantId = eventId.ToString(),
            Code = otp,
            Status = "procesado",
            CreatedAt = DateTime.UtcNow.AddMinutes(-6) // Más viejo que 5 minutos
        };
        await _dbContext.OtpRequests!.AddAsync(otpRequest);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify database state not changed to verificado
        var updatedRecord = await _dbContext.OtpRequests.FindAsync(otpRequest.Id);
        updatedRecord.Should().NotBeNull();
        updatedRecord!.Status.Should().Be("procesado");
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}