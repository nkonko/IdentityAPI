namespace identityAPI.Infrastructure.Services.FileStorage
{
    public class FileStorageSettings
    {
        /// <summary>
        /// Provider type: "Cloudinary", "Local", "AzureBlob", "S3"
        /// </summary>
        public string Provider { get; set; } = "Local";
        public CloudinarySettings? Cloudinary { get; set; }
        public LocalStorageSettings? Local { get; set; }
    }
}
