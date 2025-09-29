using identityAPI.Core.Models;
using identityAPI.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using identityAPI.Infrastructure.Persistence;
using identityAPI.Infrastructure.Services.Interfaces;

namespace identityAPI.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
            
            // Prevenir cambio de nombre del rol Admin
            if (role.Name?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true && 
                !dto.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("No se puede cambiar el nombre del rol Admin del sistema.");
            }
            
            role.Name = dto.Name;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;
            
            // Prevenir eliminación del rol Admin
            if (role.Name?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new InvalidOperationException("No se puede eliminar el rol Admin del sistema.");
            }
            
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(string id)
        {
            // Placeholder: implement permissions logic as needed
            return Task.FromResult<IEnumerable<PermissionDto>>(new List<PermissionDto>());
        }

        public Task<bool> AddRolePermissionsAsync(string id, IEnumerable<PermissionDto> dtos)
        {
            // Placeholder: implement permissions logic as needed
            return Task.FromResult(true);
        }
    }
}
