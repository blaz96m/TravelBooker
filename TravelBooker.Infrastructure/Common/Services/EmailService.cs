using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TravelBooker.Application.Common.Config;
using TravelBooker.Application.Common.Contracts.Services;
using TravelBooker.Application.Common.Models;
using TravelBooker.Application.Common.Models.Response;
using TravelBooker.Application.Logging.Contracts;

namespace TravelBooker.Infrastructure.Common.Services;

public class EmailService(IOptions<SmtpConfig> settings, ILoggerService logger) : IEmailService
{
    private readonly SmtpConfig _settings = settings.Value;
    private readonly ILoggerService _logger = logger;

    private static void ValidateEmailDetails(EmailDetails emailDetails)
    {
        if (string.IsNullOrWhiteSpace(emailDetails.ToEmail))
            throw new ArgumentException("Recipient email address is required.", nameof(emailDetails.ToEmail));
        if (!MailboxAddress.TryParse(emailDetails.ToEmail, out _))
            throw new ArgumentException($"'{emailDetails.ToEmail}' is not a valid email address.", nameof(emailDetails.ToEmail));
        if (string.IsNullOrWhiteSpace(emailDetails.Subject))
            throw new ArgumentException("Email subject is required.", nameof(emailDetails.Subject));
        if (string.IsNullOrWhiteSpace(emailDetails.HtmlBody))
            throw new ArgumentException("Email body is required.", nameof(emailDetails.HtmlBody));
    }

    private MimeMessage ComposeEmail(EmailDetails emailDetails)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(MailboxAddress.Parse(emailDetails.ToEmail));
        message.Subject = emailDetails.Subject;
        message.Body = new TextPart("html") { Text = emailDetails.HtmlBody };
        return message;
    }

    public async Task<Result> SendEmailAsync(EmailDetails emailDetails)
    {
        ValidateEmailDetails(emailDetails);
        var message = ComposeEmail(emailDetails);

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation($"Email successfully sent to {emailDetails.ToEmail}.");
            return Result.Success;
        }
        catch (SmtpCommandException ex)
        {
            _logger.LogError($"SMTP command error sending to {emailDetails.ToEmail}: {ex.Message}");
            return Result.Failure(Error.InternalError($"Failed to send email: {ex.Message}"));
        }
        catch (SmtpProtocolException ex)
        {
            _logger.LogError($"SMTP protocol error sending to {emailDetails.ToEmail}: {ex.Message}");
            return Result.Failure(Error.InternalError($"Failed to send email: {ex.Message}"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error sending email to {emailDetails.ToEmail}: {ex.Message}");
            return Result.Failure(Error.InternalError("An unexpected error occurred while sending the email."));
        }
    }
}
