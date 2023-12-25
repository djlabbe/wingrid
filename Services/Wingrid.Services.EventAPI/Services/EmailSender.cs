using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Wingrid.Services.Auth.Models;

namespace Wingrid.Services.EventAPI.Services;

public interface IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string message, int? groupId = null);
}

public class EmailService(IOptions<AuthMessageSenderOptions> optionsAccessor,
                   ILogger<EmailService> logger) : IEmailService
{
    private readonly ILogger _logger = logger;
    public AuthMessageSenderOptions Options { get; } = optionsAccessor.Value;

    public async Task SendEmailAsync(string toEmail, string subject, string message, int? groupId = null)
    {
        if (string.IsNullOrEmpty(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }

        await Execute(subject, message, toEmail, groupId);
    }

    private async Task Execute(string subject, string message, string toEmail, int? groupId = null)
    {
        var client = new SendGridClient(Options.SendGridKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("admin@wingrid.io", "Wingrid"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        if (groupId.HasValue)
        {
            msg.SetAsm(groupId.Value, [23680, 23681]);
            msg.HtmlContent += "<p><a href=\"<%asm_group_unsubscribe_raw_url%>\">Unsubscribe</a> <a href=\"<%asm_preferences_raw_url%>\">Preferences</a></p>";
        }


        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}