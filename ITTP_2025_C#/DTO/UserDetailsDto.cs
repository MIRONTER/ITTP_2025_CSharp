namespace ITTP_2025_C_.DTO
{
    public class UserDetailsDto
    {
        public string Name { get; set; }
        public int Gender { get; set; } // 0 - Женский, 1 - Мужской, 2 - Неизвестно
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
