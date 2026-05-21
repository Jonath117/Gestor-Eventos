using FluentAssertions;
using Identity.Application.Features.Users.Register;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using NSubstitute;

namespace Identity.Application.UnitTests.Features.Users.Register;

public class RegisterCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _handler = new RegisterCommandHandler(_userRepository);
    }

    [Fact]
    public async Task Handle_ShouldHashPassword_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var command = new RegisterCommand("test@example.com", "Password123!");
        _userRepository.GetByEmailAsync(command.Email).Returns((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.Received(1).AddAsync(Arg.Is<User>(u => 
            u.Email == command.Email && 
            u.PasswordHash != command.Password && 
            BCrypt.Net.BCrypt.Verify(command.Password, u.PasswordHash)), 
            Arg.Any<CancellationToken>());
        
        result.Should().NotBeNull();
        result.Email.Should().Be(command.Email);
    }
}
