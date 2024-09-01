using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AttendanceSystemAPI.Services
{
    public interface IVideoService
    {
        Task<string> UploadVideoAsync(IFormFile videoFile);
        Task<string> GetVideoResultAsync(string filename);
        Task ProcessCroppedFacesAsync(string filename);
    }
}