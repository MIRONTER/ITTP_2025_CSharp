using ITTP_2025_C_.DTO;
using ITTP_2025_C_.Models;

namespace ITTP_2025_C_.Services
{
    public interface IUserService
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(CreateUserDto dto);
        Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task<bool> UpdateUserPassAsync(Guid id, UpdatePassUserDto dto);
        Task<bool> UpdateUserLoginAsync(Guid id, UpdateLoginUserDto dto);
        Task<List<User>> GetActiveUsersAsync();
        Task<UserSummaryDto?> GetUserSummaryByLoginAsync(string login);
        Task<UserDetailsDto?> GetUserByLoginAndPasswordAsync(GetUserByLoginAndPasswordDto dto);
        Task<List<UserOverAgeDto>> GetUsersOlderThanAsync(int age); 
        Task<bool> DeleteUserAsync(Guid id, bool softDelete);
        Task<bool> RestoreUserAsync(Guid id);
    }
}
