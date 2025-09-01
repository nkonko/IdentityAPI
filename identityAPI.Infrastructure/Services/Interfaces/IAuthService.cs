using identityAPI.Core.Models;

namespace identityAPI.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateAsync(string username, string password);
        Task<AuthResponseDto?> RefreshTokenAsync(RefreshRequestDto dto);
    }
}
