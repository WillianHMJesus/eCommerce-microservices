namespace EM.Authentication.Domain.Notifications;

public interface IUserEmailNotification
{
    void SendPasswordResetEmail(string emailAddress, string securityToken);
}
