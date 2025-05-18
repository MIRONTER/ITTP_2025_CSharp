using ITTP_2025_C_.DTO;
using ITTP_2025_C_.Models;
using ITTP_2025_C_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITTP_2025_C_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        //1
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto dto)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Guid }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //2
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var success = await _userService.UpdateUserAsync(id, dto);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
        }

        //3
        [HttpPut("{id}/pass")]
        [Authorize]
        public async Task<IActionResult> UpdateUserPass(Guid id, [FromBody] UpdatePassUserDto dto)
        {
            try
            {
                var success = await _userService.UpdateUserPassAsync(id, dto);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
        }

        //4
        [HttpPut("{id}/login")]
        [Authorize]
        public async Task<IActionResult> UpdateUserLogin(Guid id, [FromBody] UpdateLoginUserDto dto)
        {
            try
            {
                var success = await _userService.UpdateUserLoginAsync(id, dto);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
        }

        //5
        [HttpGet("active")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetActiveUsers()
        {
            var users = await _userService.GetActiveUsersAsync();
            return Ok(users);
        }

        //6
        [HttpGet("summary")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserSummaryDto>> GetUserSummaryByLogin([FromQuery] string login)
        {
            var summary = await _userService.GetUserSummaryByLoginAsync(login);

            if (summary == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            return Ok(summary);
        }

        //7
        [HttpPost("login-password")]
        [Authorize]
        public async Task<ActionResult<UserDetailsDto>> GetUserByLoginAndPassword([FromBody] GetUserByLoginAndPasswordDto dto)
        {
            var userDetails = await _userService.GetUserByLoginAndPasswordAsync(dto);

            if (userDetails == null)
            {
                return NotFound(new { message = "Пользователь не найден или пароль неверен." });
            }

            return Ok(userDetails);
        }

        //8
        [HttpGet("older-than")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserOverAgeDto>>> GetUsersOlderThan([FromQuery] int age)
        {
            var users = await _userService.GetUsersOlderThanAsync(age);
            return Ok(users);
        }

        //9
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id, [FromQuery] bool softDelete = false)
        {
            var success = await _userService.DeleteUserAsync(id, softDelete);
            if (!success) return NotFound();
            return NoContent();
        }

        //10
        [HttpPatch("restore/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreUser(Guid id)
        {
            var success = await _userService.RestoreUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
