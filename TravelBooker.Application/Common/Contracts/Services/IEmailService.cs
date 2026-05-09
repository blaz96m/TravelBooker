using TravelBooker.Application.Common.Models;
using TravelBooker.Application.Common.Models.Response;

namespace TravelBooker.Application.Common.Contracts.Services;

public interface IEmailService
{
    Task<Result> SendEmailAsync(EmailDetails emailDetails);
}
