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

        //1
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

        //2
        public Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && !u.IsRevoked);
            if (user == null) return Task.FromResult(false);

            // Получаем текущего пользователя из сервиса (например, через токен)
            var currentUser = _users.FirstOrDefault(u => u.Login == dto.CurrentUserLogin && !u.IsRevoked);
            if (currentUser == null) throw new UnauthorizedAccessException("Текущий пользователь не найден или заблокирован.");

            // Проверяем права на изменение
            if (!currentUser.Admin && (currentUser.Guid != user.Guid || user.IsRevoked))
            {
                throw new UnauthorizedAccessException("Нет прав для изменения этого пользователя.");
            }

            // Обновляем поля
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

        //3
        public Task<bool> UpdateUserPassAsync(Guid id, UpdatePassUserDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && !u.IsRevoked);
            if (user == null) return Task.FromResult(false);

            // Получаем текущего пользователя из сервиса (например, через токен)
            var currentUser = _users.FirstOrDefault(u => u.Login == dto.CurrentUserLogin && !u.IsRevoked);
            if (currentUser == null) throw new UnauthorizedAccessException("Текущий пользователь не найден или заблокирован.");

            // Проверяем права на изменение
            if (!currentUser.Admin && (currentUser.Guid != user.Guid || user.IsRevoked))
            {
                throw new UnauthorizedAccessException("Нет прав для изменения этого пользователя.");
            }

            // Обновляем поля
            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = Tools.CreateSHA256(dto.Password);

            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = dto.CurrentUserLogin;

            return Task.FromResult(true);
        }

        //4
        public Task<bool> UpdateUserLoginAsync(Guid id, UpdateLoginUserDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Guid == id && !u.IsRevoked);
            if (user == null) return Task.FromResult(false);

            // Получаем текущего пользователя из сервиса (например, через токен)
            var currentUser = _users.FirstOrDefault(u => u.Login == dto.CurrentUserLogin && !u.IsRevoked);
            if (currentUser == null) throw new UnauthorizedAccessException("Текущий пользователь не найден или заблокирован.");

            // Проверяем права на изменение
            if (!currentUser.Admin && (currentUser.Guid != user.Guid || user.IsRevoked))
            {
                throw new UnauthorizedAccessException("Нет прав для изменения этого пользователя.");
            }

            // Обновляем поля
            if (!string.IsNullOrEmpty(dto.Login))
            {
                if (_users.Any(u => u.Login == dto.Login && u.Guid != user.Guid && !u.IsRevoked))
                    throw new InvalidOperationException("Логин уже используется другим пользователем.");
                user.Login = dto.Login;
            }

            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = dto.CurrentUserLogin;

            return Task.FromResult(true);
        }

        //5
        public Task<List<User>> GetActiveUsersAsync()
        {
            var activeUsers = _users
                .Where(u => !u.IsRevoked)
                .OrderByDescending(u => u.CreatedOn)
                .ToList();

            return Task.FromResult(activeUsers);
        }

        //6
        public Task<UserSummaryDto?> GetUserSummaryByLoginAsync(string login)
        {
            var user = _users.FirstOrDefault(u => u.Login == login);

            if (user == null)
            {
                return Task.FromResult<UserSummaryDto?>(null);
            }

            return Task.FromResult(new UserSummaryDto
            {
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                IsActive = !user.IsRevoked
            });
        }

        //7
        public async Task<UserDetailsDto?> GetUserByLoginAndPasswordAsync(GetUserByLoginAndPasswordDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Login == dto.Login && !u.IsRevoked);

            if (user == null || Tools.CreateSHA256(dto.Password) != user.PasswordHash)
            {
                return null;
            }

            // Получаем текущего пользователя из токена
            var currentUser = _users.FirstOrDefault(u => u.Login == dto.CurrentUserLogin && !u.IsRevoked);
            if (currentUser == null) throw new UnauthorizedAccessException("Текущий пользователь не найден или заблокирован.");

            // Проверяем, является ли текущий пользователь самим собой
            if (currentUser.Guid != user.Guid)
            {
                throw new UnauthorizedAccessException("Нет прав на получение информации о другом пользователе.");
            }

            return new UserDetailsDto
            {
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                IsActive = !user.IsRevoked,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy
            };
        }

        //8
        public Task<List<UserOverAgeDto>> GetUsersOlderThanAsync(int age)
        {
            var result = _users
                .Where(u => !u.IsRevoked && u.Age > age)
                .Select(u => new UserOverAgeDto
                {
                    Name = u.Name,
                    Age = u.Age,
                    Birthday = u.Birthday,
                    IsActive = !u.IsRevoked
                })
                .ToList();

            return Task.FromResult(result);
        }

        //9
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

        //10
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
