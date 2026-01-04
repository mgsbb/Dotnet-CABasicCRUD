using CABasicCRUD.Application.Common.Interfaces;

namespace CABasicCRUD.Infrastructure.EmailService;

internal sealed class ConsoleEmailSender : IEmailSender
{
    public Task SendWelcomeEmailAsync(
        string name,
        string email,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine("Email sent successfully");
        return Task.CompletedTask;
    }
}
