using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using identityAPI.Core.Entities;

namespace identityAPI.Core.Entities
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool Revoked { get; set; } = false;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
