using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using TravelBooker.Application.Common.Enums;
using TravelBooker.Application.Common.Models.Response;
using TravelBooker.Application.Common.Utils;
using TravelBooker.Application.User.Contracts.Repositories;
using TravelBooker.Application.User.Contracts.Services;
using TravelBooker.Application.User.Dto;
using TravelBooker.Application.User.Mapping;
using TravelBooker.Application.User.Utils;
using TravelBooker.Domain;

namespace TravelBooker.Application.User.Services
{
    public class UserAuthenticationService(
        IUserLoginRepository userLoginRepository,
        IPasswordHasher<UserLogin> passwordHasher
        ) : IUserAuthenticationService
    {
        private readonly IUserLoginRepository _userLoginRepository = userLoginRepository;
        private readonly IPasswordHasher<UserLogin> _passwordHasher = passwordHasher;

        public async Task<Result> RegisterUser(UserLoginDto userLoginDto)
        {
            var userEmailExists = await _userLoginRepository.UserLoginDataExistsAsync(userLoginDto.Email);
            if (userEmailExists)
            {
                return ResponseHelpers.GenerateFailedResult(ErrorType.Conflict, UserErrorMessages.UserEmailAlreadyExists);
            }
            var mapper = new UserLoginDomainMapper();
            var domainModel = mapper.ToDomain(userLoginDto);

            var hashedPassword = _passwordHasher.HashPassword(domainModel, domainModel.Password);
            domainModel.Password = hashedPassword;

            await using IDbContextTransaction transaction = await _userLoginRepository.BeginTransactionAsync();
            try
            {
                await _userLoginRepository.CreateAsync(domainModel);

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return Result.Success;

        }
    }
}
