using identityAPI.Core.Entities;
using identityAPI.Core.Models;
using identityAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace identityAPI.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateAsync(string username, string password);
        Task<AuthResponseDto?> RefreshTokenAsync(RefreshRequestDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                throw new UnauthorizedAccessException();
            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);
            var refreshToken = await CreateRefreshTokenAsync(user);
            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = new RefreshTokenDto { Token = refreshToken.Token, Expires = refreshToken.Expires }
            };
        }

        public async Task<AuthResponseDto?> RefreshTokenAsync(RefreshRequestDto dto)
        {
            var stored = await _context.RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken && !r.Revoked);
            if (stored == null || stored.Expires < DateTime.UtcNow)
                return null;
            var roles = await _userManager.GetRolesAsync(stored.User!);
            var newJwt = GenerateJwtToken(stored.User!, roles);
            // Opcional: revocar el refresh token usado y emitir uno nuevo
            stored.Revoked = true;
            var newRefresh = await CreateRefreshTokenAsync(stored.User!);
            await _context.SaveChangesAsync();
            return new AuthResponseDto
            {
                Token = newJwt,
                RefreshToken = new RefreshTokenDto { Token = newRefresh.Token, Expires = newRefresh.Expires }
            };
        }

        private async Task<RefreshToken> CreateRefreshTokenAsync(ApplicationUser user)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);
            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7),
                Revoked = false
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? ""));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id),
                new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
                new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email ?? "")
            };
            claims.AddRange(roles.Select(r => new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, r)));
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );
            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
