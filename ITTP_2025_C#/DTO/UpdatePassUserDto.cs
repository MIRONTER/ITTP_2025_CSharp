using System.ComponentModel.DataAnnotations;

namespace ITTP_2025_C_.DTO
{
    public class UpdatePassUserDto
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Пароль может содержать только латинские буквы и цифры.")]
        public string Password { get; set; }

        [Required]
        public string CurrentUserLogin { get; set; }
    }
}
