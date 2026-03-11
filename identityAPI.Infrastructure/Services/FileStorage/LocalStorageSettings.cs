namespace identityAPI.Infrastructure.Services.FileStorage
{
    public class LocalStorageSettings
    {
        public string BasePath { get; set; } = "uploads";
        public string BaseUrl { get; set; } = "/uploads";
    }
}
