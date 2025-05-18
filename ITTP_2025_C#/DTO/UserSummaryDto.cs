namespace ITTP_2025_C_.DTO
{
    public class UserSummaryDto
    {
        public string Name { get; set; }
        public int Gender { get; set; } // 0 - Женский, 1 - Мужской, 2 - Неизвестно
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; }
    }
}
