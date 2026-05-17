using Microsoft.AspNetCore.Identity;
using NSubstitute;
using TravelBooker.Application.Common.Contracts.Persistence;
using TravelBooker.Application.Common.Contracts.Services;
using TravelBooker.Application.Common.Enums;
using TravelBooker.Application.Common.Models;
using TravelBooker.Application.Common.Models.Response;
using TravelBooker.Application.User.Contracts.Repositories;
using TravelBooker.Application.User.Dto;
using TravelBooker.Application.User.Services;
using TravelBooker.Domain;

namespace TravelBooker.Testing.User.Services;

public class UserAuthenticationServiceTests
{
    private readonly IUserLoginRepository _repository;
    private readonly IPasswordHasher<UserLogin> _passwordHasher;
    private readonly IDataEncryptionService _encryptionService;
    private readonly ILoggerService _loggerService;
    private readonly IEmailService _emailService;
    private readonly UserAuthenticationService _sut;

    private const string GeneratedToken = "token123";
    private const string HashedPassword = "hashed";

    private static UserLoginDto DefaultDto() => new() { Email = "fresh@example.com", Password = "Password1!" };
    private static UserLogin DefaultDomainModel() => new() { Id = 1, Email = "fresh@example.com", Password = "Hashed" };

    public UserAuthenticationServiceTests()
    {
        _repository = Substitute.For<IUserLoginRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher<UserLogin>>();
        _encryptionService = Substitute.For<IDataEncryptionService>();
        _loggerService = Substitute.For<ILoggerService>();
        _emailService = Substitute.For<IEmailService>();

        _sut = new UserAuthenticationService(
            _repository, _passwordHasher, _encryptionService, _loggerService, _emailService);
    }

    private void ConfigureRegisterActionSuccess(UserLoginDto dto, UserLogin createdUser)
    {
        _repository.UserLoginDataExistsAsync(dto.Email).Returns(false);
        _passwordHasher.HashPassword(Arg.Any<UserLogin>(), Arg.Any<string>()).Returns(HashedPassword);
        _repository.CreateAsync(Arg.Any<UserLogin>(), Arg.Is(false)).Returns(createdUser);
        _encryptionService.CreateTimeLimitedEncryptionKey(Arg.Any<string>(), Arg.Any<string[]>(), Arg.Any<TimeSpan>()).Returns(GeneratedToken);
        _emailService.SendEmailAsync(Arg.Any<EmailDetails>()).Returns(Result.Success);
    }
    [Fact]
    public async Task RegisterUser_WhenEmailAlreadyExists_ReturnsConflictResult()
    {
        var dto = new UserLoginDto { Email = "existing@example.com", Password = "Password1!" };
        _repository.UserLoginDataExistsAsync(dto.Email).Returns(true);

        var result = await _sut.RegisterUser(dto);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
        await _repository.DidNotReceive().CreateAsync(Arg.Any<UserLogin>(), Arg.Is(false));

    }

    [Fact]
    public async Task RegisterUser_VerifyCorrectPasswordIsHashed()
    {
        var dto = DefaultDto();
        var createdUser = DefaultDomainModel();
        ConfigureRegisterActionSuccess(dto, createdUser);
        var result = await _sut.RegisterUser(dto);
        _passwordHasher.Received(1).HashPassword(Arg.Any<UserLogin>(), Arg.Is<string>(p => p == dto.Password));
    }

    [Fact]
    public async Task RegisterUser_VerifyCorrectTokenGenerated()
    {
        var dto = DefaultDto();
        var createdUser = DefaultDomainModel();
        ConfigureRegisterActionSuccess(dto, createdUser);
        var result = await _sut.RegisterUser(dto);
        _encryptionService.Received(1).CreateTimeLimitedEncryptionKey(
        "EmailVerification",
        Arg.Is<string[]>(arr => arr[0] == "1" && arr[1] == createdUser.Email),
        TimeSpan.FromDays(13));
    }

    [Fact]
    public async Task RegisterUser_WhenEmailIsNew_CreatesUserAndSendsVerificationEmail()
    {
        var dto = DefaultDto();
        var createdUser = DefaultDomainModel();

        ConfigureRegisterActionSuccess(dto, createdUser);

        var result = await _sut.RegisterUser(dto);
        await _emailService.Received(1).SendEmailAsync(Arg.Is<EmailDetails>(e => e.ToEmail == createdUser.Email));
    }
    [Fact]
    public async Task RegisterUser_VerifyResultSuccess()
    {
        var dto = DefaultDto();
        var createdUser = DefaultDomainModel();

        ConfigureRegisterActionSuccess(dto, createdUser);

        var result = await _sut.RegisterUser(dto);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RegisterUser_WhenEmailSendFails_StillReturnsSuccess()
    {
        var dto = DefaultDto();
        var createdUser = DefaultDomainModel();

        _repository.UserLoginDataExistsAsync(dto.Email).Returns(false);
        _passwordHasher.HashPassword(Arg.Any<UserLogin>(), Arg.Any<string>()).Returns("hashed");
        _repository.CreateAsync(Arg.Any<UserLogin>(), Arg.Is(false)).Returns(createdUser);
        _encryptionService.CreateTimeLimitedEncryptionKey(Arg.Any<string>(), Arg.Any<string[]>(), Arg.Any<TimeSpan>()).Returns("token123");
        _emailService.SendEmailAsync(Arg.Any<EmailDetails>()).Returns(Result.Failure(Error.InternalError("SMTP unavailable")));

        var result = await _sut.RegisterUser(dto);

        Assert.True(result.IsSuccess);
        _loggerService.Received(1).LogError(Arg.Any<string>());
    }
}
