namespace identityAPI.Core.Models
{
    public class AvatarUploadResponse
    {
        public bool Success { get; set; }
        public string? Url { get; set; }
        public string? PublicId { get; set; }
        public string? Error { get; set; }
    }
}
