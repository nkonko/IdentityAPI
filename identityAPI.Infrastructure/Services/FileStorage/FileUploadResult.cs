namespace identityAPI.Infrastructure.Services.FileStorage
{
    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
