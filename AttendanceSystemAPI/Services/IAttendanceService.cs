using System.Collections.Generic;
using System.Threading.Tasks;
using AttendanceSystemAPI.Models;

namespace AttendanceSystemAPI.Services
{
    public interface IAttendanceService
    {
        Task<(List<Person> persons, bool emailsSent, int emailsSentCount, int unknownCount)> GetAttendanceResultAsync(string filename);
    }
}