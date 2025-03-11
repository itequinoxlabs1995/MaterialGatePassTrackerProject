using MaterialGatePassTacker.Models;

namespace MaterialGatePassTracker.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string htmlBody, T_Gate_Pass request, List<string> filePaths);
    }
}
