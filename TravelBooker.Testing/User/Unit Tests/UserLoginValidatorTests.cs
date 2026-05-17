using TravelBooker.Application.User.Dto;
using TravelBooker.Application.User.Validation;

namespace TravelBooker.Testing.User.Validation;

public class UserLoginValidatorTests
{
    private readonly UserLoginValidator _validator = new();

    [Fact]
    public void Validate_WithValidEmailAndPassword_Passes()
    {
        var dto = new UserLoginDto { Email = "user@example.com", Password = "Password1!" };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    [InlineData("@nodomain.com")]
    public void Validate_WithInvalidEmail_FailsOnEmailProperty(string email)
    {
        var dto = new UserLoginDto { Email = email, Password = "Password1!" };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserLoginDto.Email));
    }

    [Fact]
    public void Validate_WithEmptyPassword_FailsOnPasswordProperty()
    {
        var dto = new UserLoginDto { Email = "user@example.com", Password = "" };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserLoginDto.Password));
    }

    [Fact]
    public void Validate_WithPasswordTooShort_Fails()
    {
        var dto = new UserLoginDto { Email = "user@example.com", Password = "Ab1!" };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserLoginDto.Password));
    }

    [Fact]
    public void Validate_WithPasswordMissingUppercase_Fails()
    {
        var dto = new UserLoginDto { Email = "user@example.com", Password = "password1!" };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserLoginDto.Password));
    }

    [Fact]
    public void Validate_WithPasswordMissingDigit_Fails()
    {
        var dto = new UserLoginDto { Email = "user@example.com", Password = "Password!" };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserLoginDto.Password));
    }

    [Fact]
    public void Validate_WithPasswordMissingSpecialChar_Fails()
    {
        var dto = new UserLoginDto { Email = "user@example.com", Password = "Password1" };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserLoginDto.Password));
    }

    [Theory]
    [InlineData("!", true)]
    [InlineData("@", true)]
    [InlineData("#abc", true)]
    [InlineData("abc", false)]
    [InlineData("abc123", false)]
    public void SpecialCharacterExists_ReturnsExpectedResult(string input, bool expected)
    {
        var result = UserLoginValidator.SpecialCharacterExists(input);

        Assert.Equal(expected, result);
    }
}
