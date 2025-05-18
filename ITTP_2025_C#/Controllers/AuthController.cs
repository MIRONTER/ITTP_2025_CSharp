using ITTP_2025_C_.DTO;
using ITTP_2025_C_.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITTP_2025_C_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenService _tokenService;

        public AuthController(IUserService userService, JwtTokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var users = await _userService.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Login == dto.Login && !u.IsRevoked);

            if (user == null || Tools.CreateSHA256(dto.Password) != user.PasswordHash)
                return Unauthorized(new { error = "Invalid login or password" });

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
