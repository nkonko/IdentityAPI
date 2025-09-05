namespace identityAPI.Core.Models
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        public UserStatus Status { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }

    public class UserCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserStatus Status { get; set; } = UserStatus.Active;
    }

    public class UserUpdateDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public UserStatus Status { get; set; }
    }

    public class UserPasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Blocked,
    }
}
