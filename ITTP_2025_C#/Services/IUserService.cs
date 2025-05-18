using ITTP_2025_C_.DTO;
using ITTP_2025_C_.Models;

namespace ITTP_2025_C_.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetUserByIdAsync(Guid id);
        public Task<User> CreateUserAsync(CreateUserDto dto);
        public Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto);
        public Task<bool> DeleteUserAsync(Guid id, bool softDelete);
        public Task<bool> RestoreUserAsync(Guid id);
    }
}
