using identityAPI.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using identityAPI.Infrastructure.Persistence;

namespace identityAPI.Infrastructure.Services
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

    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RoleService(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var dtos = new List<RoleDto>();
            foreach (var role in roles)
            {
                dtos.Add(new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name ?? "",
                    Permissions = new List<PermissionDto>() // Placeholder
                });
            }
            return dtos;
        }

        public async Task<RoleDto> CreateRoleAsync(RoleCreateDto dto)
        {
            var role = new IdentityRole(dto.Name);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception("Role creation failed");
            return new RoleDto { Id = role.Id, Name = role.Name ?? "", Permissions = new List<PermissionDto>() };
        }

        public async Task<bool> UpdateRoleAsync(string id, RoleUpdateDto dto)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;
            role.Name = dto.Name;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(string id)
        {
            // Placeholder: implement permissions logic as needed
            return new List<PermissionDto>();
        }

        public async Task<bool> AddRolePermissionsAsync(string id, IEnumerable<PermissionDto> dtos)
        {
            // Placeholder: implement permissions logic as needed
            return true;
        }
    }
}
