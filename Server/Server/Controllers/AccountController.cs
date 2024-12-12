using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;

        public AccountController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            if (register == null)
            {
                return BadRequest(new { Success = false, Message = "Invalid user data." });
            }

            var result = await _firebaseService.RegisterUser(register);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Success = false, Message = "Invalid login data." });
            }

            var result = await _firebaseService.LoginUser(request.Username, request.Password);
            return Ok(result);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest(new { Success = false, Message = "Invalid request data." });
            }

            var result = await _firebaseService.UpdatePasswordInFirebase(request.Username, request.NewPassword);
            return Ok(result);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UpdatePasswordRequest
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
    }
}
