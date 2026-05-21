using Core.Domain.Entities;
using FluentAssertions;

namespace Core.Domain.UnitTests.Entities;

public class EventTests
{
    private readonly string _validName = "Rock Festival 2026";
    private readonly DateTime _startDate = DateTime.UtcNow.AddDays(10);
    private readonly DateTime _endDate = DateTime.UtcNow.AddDays(11);
    private readonly int _validCapacity = 500;
    private readonly Guid _organizationId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidParameters_ShouldReturnEvent()
    {
        // Act
        Event result = Event.Create(_validName, _startDate, _endDate, _validCapacity, _organizationId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(_validName);
        result.StartDate.Should().Be(_startDate);
        result.EndDate.Should().Be(_endDate);
        result.MaxCapacity.Should().Be(_validCapacity);
        result.OrganizationId.Should().Be(_organizationId);
        result.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string? invalidName)
    {
        // Act
        Action act = () => Event.Create(invalidName!, _startDate, _endDate, _validCapacity, _organizationId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*name*");
    }

    [Fact]
    public void Create_WithEndDateBeforeStartDate_ShouldThrowArgumentException()
    {
        // Arrange
        DateTime invalidEndDate = _startDate.AddHours(-1);

        // Act
        Action act = () => Event.Create(_validName, _startDate, invalidEndDate, _validCapacity, _organizationId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*End date must be after start date.*");
    }

    [Fact]
    public void Create_WithZeroCapacity_ShouldThrowArgumentException()
    {
        // Act
        Action act = () => Event.Create(_validName, _startDate, _endDate, 0, _organizationId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*capacity*");
    }
}
