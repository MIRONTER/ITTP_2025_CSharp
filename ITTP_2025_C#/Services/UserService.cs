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
                Login = "Admin",
                PasswordHash = Tools.CreateSHA256("adminpass"),
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
                Admin = false,
                CreatedBy = dto.CurrentUserLogin
            };

            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && !u.IsRevoked);
            if (user == null) return Task.FromResult(false);

            if (!string.IsNullOrEmpty(dto.Login) &&
                _users.Any(u => u.Guid != id && u.Login == dto.Login && !u.IsRevoked))
                throw new InvalidOperationException("Логин уже используется другим пользователем.");

            user.Login = dto.Login ?? user.Login;
            user.PasswordHash = !string.IsNullOrEmpty(dto.Password) ? Tools.CreateSHA256(dto.Password) : user.PasswordHash;
            user.Name = dto.Name ?? user.Name;
            user.Gender = dto.Gender ?? user.Gender;
            user.Birthday = dto.Birthday ?? user.Birthday;

            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = dto.CurrentUserLogin;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteUserAsync(Guid id, DeleteUserDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && !u.IsRevoked);
            if (user == null) return Task.FromResult(false);

            user.RevokedOn = DateTime.UtcNow;
            user.RevokedBy = dto.CurrentUserLogin;

            return Task.FromResult(true);
        }
    }
}