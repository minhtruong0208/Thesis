using System.Threading.Tasks;

namespace AttendanceSystemAPI.Services
{
    public interface IClassService
    {
        Task<string> GetClassNameAsync(string classId);
    }
}