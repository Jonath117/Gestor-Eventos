using FluentAssertions;
using Identity.Application.Features.Users.RefreshToken;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using NSubstitute;

namespace Identity.Application.UnitTests.Features.Users.RefreshToken;

public class RefreshTokenCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IJwtTokenGenerator _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
    private readonly RefreshTokenCommandHandler _handler;

    public RefreshTokenCommandHandlerTests()
    {
        _handler = new RefreshTokenCommandHandler(_userRepository, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ShouldRotateTokens_WhenRefreshTokenIsActive()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "test@example.com", "hash");
        var oldToken = "old-refresh-token";
        user.AddRefreshToken(oldToken, DateTime.UtcNow.AddDays(1));
        
        var command = new RefreshTokenCommand(oldToken);
        _userRepository.GetByRefreshTokenAsync(oldToken).Returns(user);
        
        _jwtTokenGenerator.GenerateRefreshToken().Returns("new-refresh-token");
        _jwtTokenGenerator.GenerateToken(user.Id, user.Email, Arg.Any<string>(), Arg.Any<string>()).Returns("new-access-token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("new-access-token");
        result.RefreshToken.Should().Be("new-refresh-token");
        
        var oldRt = user.RefreshTokens.Single(x => x.Token == oldToken);
        oldRt.IsRevoked.Should().BeTrue();
        oldRt.ReplacedByToken.Should().Be("new-refresh-token");
        
        user.RefreshTokens.Should().ContainSingle(x => x.Token == "new-refresh-token");
        await _userRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenTokenIsExpired()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "test@example.com", "hash");
        var expiredToken = "expired-token";
        // Adding expired token (ExpiryDate in the past)
        user.AddRefreshToken(expiredToken, DateTime.UtcNow.AddMinutes(-1));
        
        var command = new RefreshTokenCommand(expiredToken);
        _userRepository.GetByRefreshTokenAsync(expiredToken).Returns(user);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Refresh token is not active");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenTokenIsRevoked()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "test@example.com", "hash");
        var revokedToken = "revoked-token";
        user.AddRefreshToken(revokedToken, DateTime.UtcNow.AddDays(1));
        user.RevokeRefreshToken(revokedToken);
        
        var command = new RefreshTokenCommand(revokedToken);
        _userRepository.GetByRefreshTokenAsync(revokedToken).Returns(user);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Refresh token is not active");
    }
}
