using ITTP_2025_C_.DTO;
using ITTP_2025_C_.Models;

namespace ITTP_2025_C_.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();

        public UserService()
        {
            // Преднастроенный пользователь Admin
            var adminUser = new User
            {
                Login = "admin",
                PasswordHash = Tools.CreateSHA256("admin"),
                Name = "Administrator",
                Gender = 2,
                Admin = true,
                CreatedBy = "System"
            };
            _users.Add(adminUser);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_users.Where(u => !u.IsRevoked).ToList());
        }

        public Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && !u.IsRevoked);
            return Task.FromResult(user);
        }

        public Task<User> CreateUserAsync(CreateUserDto dto)
        {
            if (_users.Any(u => u.Login == dto.Login && !u.IsRevoked))
                throw new InvalidOperationException("Пользователь с таким логином уже существует.");

            var passwordHash = Tools.CreateSHA256(dto.Password);

            var user = new User
            {
                Login = dto.Login,
                PasswordHash = passwordHash,
                Name = dto.Name,
                Gender = dto.Gender,
                Birthday = dto.Birthday,
                Admin = dto.Admin,
                CreatedBy = dto.CurrentUserLogin
            };

            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && !u.IsRevoked);
            if (user == null) return Task.FromResult(false);

            if (!string.IsNullOrEmpty(dto.Login))
            {
                if (_users.Any(u => u.Login == dto.Login && u.Guid != user.Guid && !u.IsRevoked))
                    throw new InvalidOperationException("Логин уже используется другим пользователем.");
                user.Login = dto.Login;
            }

            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = Tools.CreateSHA256(dto.Password);

            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;

            if (dto.Gender.HasValue)
                user.Gender = dto.Gender.Value;

            if (dto.Birthday.HasValue)
                user.Birthday = dto.Birthday;

            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = dto.CurrentUserLogin;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteUserAsync(Guid id, bool softDelete)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && !u.IsRevoked);
            if (user == null) return Task.FromResult(false);

            if (softDelete)
            {
                user.RevokedOn = DateTime.UtcNow;
                user.RevokedBy = "CurrentUser";
            }
            else
            {
                _users.Remove(user);
            }

            return Task.FromResult(true);
        }

        public Task<bool> RestoreUserAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && u.IsRevoked);
            if (user == null) return Task.FromResult(false);

            user.RevokedOn = null;
            user.RevokedBy = null;

            return Task.FromResult(true);
        }
    }
}
