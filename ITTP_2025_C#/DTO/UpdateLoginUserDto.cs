using System.ComponentModel.DataAnnotations;

namespace ITTP_2025_C_.DTO
{
    public class UpdateLoginUserDto
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Login может содержать только латинские буквы и цифры.")]
        public string Login { get; set; }

        [Required]
        public string CurrentUserLogin { get; set; }
    }
}
