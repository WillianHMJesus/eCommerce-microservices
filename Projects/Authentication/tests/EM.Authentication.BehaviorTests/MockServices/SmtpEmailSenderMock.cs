using System.Net;
using System.Net.Mail;
using WH.SharedKernel.Notifications;

namespace EM.Authentication.BehaviorTests.MockServices;

internal class SmtpEmailSenderMock : IEmailSender
{
    public void Send(MailMessage message, string host, int port, NetworkCredential? credential = null)
    {
        return;
    }
}
