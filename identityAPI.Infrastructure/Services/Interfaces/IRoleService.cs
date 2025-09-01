using identityAPI.Core.Models;

namespace identityAPI.Infrastructure.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        Task<RoleDto> CreateRoleAsync(RoleCreateDto dto);
        Task<bool> UpdateRoleAsync(string id, RoleUpdateDto dto);
        Task<bool> DeleteRoleAsync(string id);
        Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(string id);
        Task<bool> AddRolePermissionsAsync(string id, IEnumerable<PermissionDto> dtos);
    }
}
