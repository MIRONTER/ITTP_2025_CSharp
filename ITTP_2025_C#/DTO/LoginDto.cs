using System.ComponentModel.DataAnnotations;

namespace ITTP_2025_C_.DTO
{
    public class LoginDto
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
