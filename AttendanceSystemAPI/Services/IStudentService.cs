using System.Collections.Generic;
using System.Threading.Tasks;
using AttendanceSystemAPI.Models;

namespace AttendanceSystemAPI.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task UpdateStudentAsync(string id, Student updatedStudent);
        Task<string> GetStudentImageAsync(string studentName);
    }
}