using System.ComponentModel.DataAnnotations;

namespace ITTP_2025_C_.DTO
{
    public class CreateUserDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Login может содержать только латинские буквы и цифры.")]
        public string Login { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Пароль может содержать только латинские буквы и цифры.")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ]+$", ErrorMessage = "Имя может содержать только латинские и кириллические буквы.")]
        public string Name { get; set; }

        [Range(0, 2)]
        public int Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public bool Admin { get; set; }

        [Required]
        public string CurrentUserLogin { get; set; } // Кто создает
    }
}
