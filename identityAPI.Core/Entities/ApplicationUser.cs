using identityAPI.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace identityAPI.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
