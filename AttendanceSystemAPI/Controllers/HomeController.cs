using Microsoft.AspNetCore.Mvc;
using AttendanceSystemAPI.Models;
using AttendanceSystemAPI.Services;
using System.Threading.Tasks;
using System.IO;

namespace AttendanceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IAttendanceService _attendanceService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IVideoService videoService,
            IAttendanceService attendanceService,
            IStudentService studentService,
            IClassService classService,
            ILogger<HomeController> logger)
        {
            _videoService = videoService;
            _attendanceService = attendanceService;
            _studentService = studentService;
            _classService = classService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo(IFormFile videoFile)
        {
            var fileName = await _videoService.UploadVideoAsync(videoFile);
            return Ok(new { FileName = fileName });
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> GetVideoResult(string filename)
        {
            var videoPath = await _videoService.GetVideoResultAsync(filename);
            return PhysicalFile(videoPath, "video/mp4");
        }

        [HttpGet("output/processed_{filename}")]
        public IActionResult GetOutputVideo(string filename)
        {
            var outputVideoPath = Path.Combine(Directory.GetCurrentDirectory(), "videos", $"processed_{filename}");
            if (!System.IO.File.Exists(outputVideoPath))
            {
                return NotFound($"Processed video not found: {filename}");
            }
            return PhysicalFile(outputVideoPath, "video/mp4");
        }

        [HttpPost("process-cropped-faces/processed_{filename}")]
        public async Task<IActionResult> ProcessCroppedFaces(string filename)
        {
            await _videoService.ProcessCroppedFacesAsync(filename);
            return Ok();
        }

        [HttpGet("result/processed_{filename}")]
        public async Task<IActionResult> DisplayResult(string filename)
        {
            var (persons, emailsSent, emailsSentCount, unknownCount) = await _attendanceService.GetAttendanceResultAsync(filename);
            return Ok(new { persons, emailsSent, emailsSentCount, unknownCount });
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpPut("students/{id}")]
        public async Task<IActionResult> UpdateStudent(string id, [FromBody] Student updatedStudent)
        {
            await _studentService.UpdateStudentAsync(id, updatedStudent);
            return NoContent();
        }

        [HttpGet("classes/{classId}")]
        public async Task<IActionResult> GetClassById(string classId)
        {
            var className = await _classService.GetClassNameAsync(classId);
            if (className == null)
            {
                return NotFound();
            }
            return Ok(new { ClassName = className });
        }

        [HttpGet("student-image/{studentName}")]
        public async Task<IActionResult> GetStudentImage(string studentName)
        {
            var imagePath = await _studentService.GetStudentImageAsync(studentName);
            if (imagePath == null)
            {
                return NotFound();
            }
            return PhysicalFile(imagePath, "image/jpeg");
        }

        [HttpGet("unknown-faces/{fileName}")]
        public IActionResult GetUnknownFaceImage(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "UnknownFaces", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            return PhysicalFile(filePath, "image/jpeg");
        }
    }
}