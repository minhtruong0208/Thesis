using System.Threading.Tasks;

namespace AttendanceSystemAPI.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}