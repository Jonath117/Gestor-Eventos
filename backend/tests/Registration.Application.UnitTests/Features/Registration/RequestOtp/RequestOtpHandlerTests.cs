using System;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NSubstitute;

using Registration.Application.Features.Registration.RequestOtp;
using Registration.Infrastructure.Database;

using Xunit;

namespace Registration.Application.UnitTests.Features.Registration.RequestOtp;

public class RequestOtpHandlerTests : IDisposable
{
    private readonly RegistrationDbContext _dbContext;
    private readonly ILogger<RequestOtpHandler> _logger = Substitute.For<ILogger<RequestOtpHandler>>();
    private readonly RequestOtpHandler _handler;

    public RequestOtpHandlerTests()
    {
        var options = new DbContextOptionsBuilder<RegistrationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new RegistrationDbContext(options);
        _handler = new RequestOtpHandler(_dbContext, _logger);
    }

    [Fact]
    public async Task Handle_ShouldStorePendingOtpRequestInDatabase()
    {
        // Arrange
        var command = new RequestOtpCommand(Guid.NewGuid(), "test@example.com", "John Doe");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var otpRequest = await _dbContext.OtpRequests!.FirstOrDefaultAsync(o => o.UserId == command.Email);
        otpRequest.Should().NotBeNull();
        otpRequest!.UserId.Should().Be(command.Email);
        otpRequest.TenantId.Should().Be(command.EventId.ToString());
        otpRequest.Status.Should().Be("pendiente");
        otpRequest.Code.Should().BeNull();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}