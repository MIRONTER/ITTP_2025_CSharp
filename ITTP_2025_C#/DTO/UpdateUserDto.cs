using System.ComponentModel.DataAnnotations;

namespace ITTP_2025_C_.DTO
{
    public class UpdateUserDto
    {
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ]+$", ErrorMessage = "Имя может содержать только латинские и кириллические буквы.")]
        public string Name { get; set; }

        [Range(0, 2, ErrorMessage = "Пол должен быть: 0 - Женский, 1 - Мужской, 2 - Неизвестно")]
        public int? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        [Required]
        public string CurrentUserLogin { get; set; }
    }
}
