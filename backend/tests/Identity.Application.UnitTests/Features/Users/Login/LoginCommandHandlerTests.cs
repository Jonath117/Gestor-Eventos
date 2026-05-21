using FluentAssertions;
using Identity.Application.Features.Users.Login;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Identity.Domain.Repositories;
using NSubstitute;

namespace Identity.Application.UnitTests.Features.Users.Login;

public class LoginCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IJwtTokenGenerator _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _handler = new LoginCommandHandler(_userRepository, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        // Arrange
        var password = "Password123!";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User(Guid.NewGuid(), "test@example.com", passwordHash);
        
        var command = new LoginCommand(user.Email, password);
        
        _userRepository.GetByEmailAsync(command.Email).Returns(user);
        _jwtTokenGenerator.GenerateToken(user.Id, user.Email, Arg.Any<string>(), Arg.Any<string>()).Returns("access-token");
        _jwtTokenGenerator.GenerateRefreshToken().Returns("refresh-token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");
        
        user.RefreshTokens.Should().ContainSingle(x => x.Token == "refresh-token");
        await _userRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidCredentialsException_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new LoginCommand("nonexistent@example.com", "any-password");
        _userRepository.GetByEmailAsync(command.Email).Returns((User?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidCredentialsException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "test@example.com", BCrypt.Net.BCrypt.HashPassword("CorrectPassword"));
        var command = new LoginCommand(user.Email, "WrongPassword");
        
        _userRepository.GetByEmailAsync(command.Email).Returns(user);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }
}
