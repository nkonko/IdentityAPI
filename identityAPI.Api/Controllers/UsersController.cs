using identityAPI.Core.Models;
using identityAPI.Infrastructure.Services.Interfaces;
using identityAPI.Infrastructure.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace identityAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileStorageService _fileStorageService;

        public UsersController(IUserService userService, IFileStorageService fileStorageService)
        {
            _userService = userService;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var result = await _userService.UpdateUserAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetMe()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            var user = await _userService.GetUserByIdAsync(userId);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPatch("me/password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            
            var result = await _userService.ChangePasswordAsync(userId, dto);
            return result ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Upload avatar for the current user
        /// </summary>
        [HttpPatch("me/avatar")]
        public async Task<ActionResult<AvatarUploadResponse>> UploadMyAvatar(IFormFile file)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            return await UploadAvatarInternal(userId, file);
        }

        /// <summary>
        /// Delete avatar for the current user
        /// </summary>
        [HttpDelete("me/avatar")]
        public async Task<IActionResult> DeleteMyAvatar()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            return await DeleteAvatarInternal(userId);
        }

        private async Task<ActionResult<AvatarUploadResponse>> UploadAvatarInternal(string userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new AvatarUploadResponse { Success = false, Error = "No file provided" });

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLowerInvariant()))
                return BadRequest(new AvatarUploadResponse { Success = false, Error = "Invalid file type. Allowed: jpeg, png, gif, webp" });

            // Validate file size (max 5MB)
            const long maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize)
                return BadRequest(new AvatarUploadResponse { Success = false, Error = "File too large. Maximum size is 5MB" });

            using var stream = file.OpenReadStream();
            var result = await _fileStorageService.UploadAsync(stream, file.FileName, "avatars");

            if (!result.Success)
                return BadRequest(new AvatarUploadResponse { Success = false, Error = result.Error });

            // Update user's profile picture URL
            var user = await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                var updateDto = new UserUpdateDto
                {
                    Email = user.Email,
                    Name = user.Name,
                    Position = user.Position,
                    Bio = user.Bio,
                    ProfilePictureUrl = result.Url,
                    Status = user.Status
                };
                await _userService.UpdateUserAsync(userId, updateDto);
            }

            return Ok(new AvatarUploadResponse
            {
                Success = true,
                Url = result.Url,
                PublicId = result.PublicId
            });
        }

        private async Task<IActionResult> DeleteAvatarInternal(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                // Try to delete from storage (extract publicId from URL if needed)
                await _fileStorageService.DeleteAsync(user.ProfilePictureUrl);

                // Clear the profile picture URL
                var updateDto = new UserUpdateDto
                {
                    Email = user.Email,
                    Name = user.Name,
                    Position = user.Position,
                    Bio = user.Bio,
                    ProfilePictureUrl = null,
                    Status = user.Status
                };
                await _userService.UpdateUserAsync(userId, updateDto);
            }

            return NoContent();
        }
    }
}
