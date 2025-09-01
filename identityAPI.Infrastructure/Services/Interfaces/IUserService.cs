using identityAPI.Core.Models;

namespace identityAPI.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<UserDto> CreateUserAsync(UserCreateDto dto);
        Task<bool> UpdateUserAsync(string id, UserUpdateDto dto);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> ChangePasswordAsync(string userId, UserPasswordDto dto);
    }
}
