using FluentAssertions;
using Identity.Application.Features.Users.Logout;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using NSubstitute;

namespace Identity.Application.UnitTests.Features.Users.Logout;

public class LogoutCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly LogoutCommandHandler _handler;

    public LogoutCommandHandlerTests()
    {
        _handler = new LogoutCommandHandler(_userRepository);
    }

    [Fact]
    public async Task Handle_ShouldRevokeToken_WhenTokenIsValid()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "test@example.com", "hash");
        var token = "valid-token";
        user.AddRefreshToken(token, DateTime.UtcNow.AddDays(1));
        
        var command = new LogoutCommand(token);
        _userRepository.GetByRefreshTokenAsync(token).Returns(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        user.RefreshTokens.Single(x => x.Token == token).IsRevoked.Should().BeTrue();
        await _userRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldDoNothing_WhenTokenDoesNotExist()
    {
        // Arrange
        var command = new LogoutCommand("nonexistent-token");
        _userRepository.GetByRefreshTokenAsync(command.Token).Returns((User?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }
}
