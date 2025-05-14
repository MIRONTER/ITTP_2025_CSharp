namespace ITTP_2025_C_.Models
{
    public class User
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; } // 0 - Женский, 1 - Мужской, 2 - Неизвестно
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }

        public DateTime? RevokedOn { get; set; }
        public string RevokedBy { get; set; }

        public bool IsRevoked => RevokedOn.HasValue;
    }
}
