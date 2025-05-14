using System.ComponentModel.DataAnnotations;

namespace ITTP_2025_C_.DTO
{
    public class DeleteUserDto
    {
        [Required]
        public string CurrentUserLogin { get; set; } // Кто удаляет
    }
}