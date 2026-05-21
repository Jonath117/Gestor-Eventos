using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using NSubstitute;

using Registration.Application.Features.Registration.SubmitRegistration;
using Registration.Application.Interfaces;
using Registration.Domain.Entities;

using Xunit;

namespace Registration.Application.UnitTests.Features.Registration.SubmitRegistration;

public class SubmitRegistrationHandlerTests
{
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly IRegistrationDbContext _dbContext = Substitute.For<IRegistrationDbContext>();
    private readonly SubmitRegistrationHandler _handler;

    public SubmitRegistrationHandlerTests()
    {
        _handler = new SubmitRegistrationHandler(_cache, _dbContext);
    }

    [Fact]
    public async Task Handle_ShouldSaveRegistration_WhenVerified()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var email = "test@example.com";
        var command = new SubmitRegistrationCommand(eventId, email, "John Doe", "77788899");
        var verifiedKey = $"Verified_{eventId}_{email}";

        _cache.Set(verifiedKey, true);

        var ordersSet = Substitute.For<DbSet<Order>, IQueryable<Order>>();
        _dbContext.Orders.Returns(ordersSet);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await ordersSet.Received(1).AddAsync(Arg.Is<Order>(o =>
            o.EventId == eventId &&
            o.ContactEmail == email &&
            o.Participants.Count == 1 &&
            o.Participants.First().FullName == command.FullName),
            Arg.Any<CancellationToken>());
        _cache.TryGetValue(verifiedKey, out _).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenNotVerified()
    {
        // Arrange
        var command = new SubmitRegistrationCommand(Guid.NewGuid(), "test@example.com", "John Doe", null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("OTP verification required or expired.");
    }
}