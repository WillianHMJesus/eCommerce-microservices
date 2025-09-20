using EM.Authentication.Domain.Notifications;
using EM.Authentication.Infraestructure.SettingsOptions;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using WH.SharedKernel.Notifications;

namespace EM.Authentication.Infraestructure.Notifications;

public sealed class UserEmailNotification : IUserEmailNotification
{
    private readonly ISmtpEmailSender _emailSender;
    private readonly EmailNotificationOptions _emailNotificationOptions;

    public UserEmailNotification(
        ISmtpEmailSender emailSender,
        IOptions<EmailNotificationOptions> options)
    {
        _emailSender = emailSender;
        _emailNotificationOptions = options.Value;
    }

    public void SendPasswordResetEmail(string emailAddress, string securityToken)
    {
        MailMessage mail = new MailMessage()
        {
            From = new MailAddress(_emailNotificationOptions.From.EmailAddress, _emailNotificationOptions.From.Name),
            Subject = "Password Reset",
            Body = GetPasswordResetEmailTemplate(securityToken),
            IsBodyHtml = true
        };
        mail.To.Add(emailAddress);
        var credentials = new NetworkCredential(_emailNotificationOptions.Credentials.UserName, _emailNotificationOptions.Credentials.Password);

        _emailSender.Send(mail, _emailNotificationOptions.Smtp, _emailNotificationOptions.Port, credentials);
    }

    private string GetPasswordResetEmailTemplate(string securityToken) =>
        $@"
            <html>
            <body>
                <h1>Hi!</h1>
                <p>It's here your security token <strong>{securityToken}</strong>.</p>
            </body>
            </html>";
}
