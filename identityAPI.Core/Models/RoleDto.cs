namespace identityAPI.Core.Models
{
    public class RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public IList<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }

    public class RoleCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class RoleUpdateDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class PermissionDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
