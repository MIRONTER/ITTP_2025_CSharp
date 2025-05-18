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

        [HttpPost]
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

        [HttpPut("{id}")]
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
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id, [FromQuery] bool softDelete = false)
        {
            var success = await _userService.DeleteUserAsync(id, softDelete);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPatch("restore/{id}")]
        public async Task<IActionResult> RestoreUser(Guid id)
        {
            var success = await _userService.RestoreUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
