using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using AttendanceSystemAPI.Models;

namespace AttendanceSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMongoClient mongoClient, ILogger<AuthController> logger)
        {
            var database = mongoClient.GetDatabase("AttendanceSystem");
            _users = database.GetCollection<User>("users");
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"Attempting login for username: {request.Username}");

            var user = _users.Find(u => u.Username == request.Username && u.Password == request.Password).FirstOrDefault();

            if (user == null)
            {
                _logger.LogWarning($"Failed login attempt for username: {request.Username}");
                return Unauthorized("Invalid username or password.");
            }

            _logger.LogInformation($"Successful login for username: {request.Username}");
            return Ok(new
            {
                Username = user.Username,
                Roles = user.Roles
            });
        }
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            _logger.LogInformation($"Attempting reset for username: {request.Username}");
            var user = _users.Find(u => u.Username == request.Username).FirstOrDefault();

            if (user == null)
            {
                _logger.LogWarning($"Failed reset attempt for username: {request.Username}");
                return NotFound("Tài khoản không tồn tại");
            }

            user.Password = "abcd1234"; // Đổi mật khẩu thành "abcd1234"
            _users.ReplaceOne(u => u.Username == request.Username, user);

            return Ok("Đổi mật khẩu thành công");
        }
    }
}
