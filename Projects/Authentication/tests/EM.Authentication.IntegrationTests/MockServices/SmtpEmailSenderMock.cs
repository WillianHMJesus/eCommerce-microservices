using System.Net;
using System.Net.Mail;
using WH.SharedKernel.Notifications;

namespace EM.Authentication.IntegrationTests.MockServices;

internal class SmtpEmailSenderMock : ISmtpEmailSender
{
    public void Send(MailMessage message, string host, int port, NetworkCredential? credential = null)
    {
        return;
    }
}
