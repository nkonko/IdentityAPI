using identityAPI.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace identityAPI.Infrastructure.Services.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly LocalStorageSettings _settings;

        public LocalFileStorageService(IOptions<FileStorageSettings> settings)
        {
            _settings = settings.Value.Local ?? new LocalStorageSettings();
            
            if (!Directory.Exists(_settings.BasePath))
            {
                Directory.CreateDirectory(_settings.BasePath);
            }
        }

        public async Task<FileUploadResult> UploadAsync(Stream fileStream, string fileName, string? folder = null)
        {
            try
            {
                var folderPath = string.IsNullOrEmpty(folder) 
                    ? _settings.BasePath 
                    : Path.Combine(_settings.BasePath, folder);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var extension = Path.GetExtension(fileName);
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStreamOutput);
                }

                var relativePath = string.IsNullOrEmpty(folder)
                    ? uniqueFileName
                    : $"{folder}/{uniqueFileName}";

                var url = $"{_settings.BaseUrl}/{relativePath}";

                return new FileUploadResult
                {
                    Success = true,
                    Url = url,
                    PublicId = relativePath
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResult
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        public Task<bool> DeleteAsync(string fileIdentifier)
        {
            try
            {
                var filePath = Path.Combine(_settings.BasePath, fileIdentifier);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
