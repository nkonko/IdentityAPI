using identityAPI.Core.Models;
using identityAPI.Core.Entities;
using identityAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using identityAPI.Infrastructure.Services.Interfaces;

namespace identityAPI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                dtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Roles = roles,
                    Bio = user.Bio ?? "",
                    Name = user.Name,
                    Position = user.Position,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    LastLogin = user.LastLogin,
                    Status = user.Status
                });
            }
            return dtos;
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Roles = roles,
                Bio = user.Bio ?? "",
                Name = user.Name,
                Position = user.Position,
                ProfilePictureUrl = user.ProfilePictureUrl,
                LastLogin = user.LastLogin,
                Status = user.Status
            };
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                Name = dto.Name,
                Status = dto.Status
            }; ;
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception("User creation failed");
            await _userManager.AddToRoleAsync(user, "User");
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = user.Id,
                Name= user.Name ?? "",
                Email = user.Email ?? "",
                Roles = roles,
                Status = UserStatus.Active
            };
        }

        public async Task<bool> UpdateUserAsync(string id, UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            user.Email = dto.Email;
            user.Name = dto.Name;
            user.Bio  = dto.Bio;
            user.ProfilePictureUrl = dto.ProfilePictureUrl;
            user.Position  = dto.Position;
            user.Status = dto.Status;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(string userId, UserPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            return result.Succeeded;
        }
    }
}
