namespace CABasicCRUD.Application.Common.Interfaces;

public interface IEmailSender
{
    Task SendWelcomeEmailAsync(string name, string email, CancellationToken cancellationToken);
}
