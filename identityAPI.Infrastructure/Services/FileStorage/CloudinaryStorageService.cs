using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using identityAPI.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace identityAPI.Infrastructure.Services.FileStorage
{
    public class CloudinaryStorageService : IFileStorageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryStorageService(IOptions<FileStorageSettings> settings)
        {
            var cloudinarySettings = settings.Value.Cloudinary 
                ?? throw new ArgumentNullException("Cloudinary settings are required");

            var account = new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<FileUploadResult> UploadAsync(Stream fileStream, string fileName, string? folder = null)
        {
            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, fileStream),
                    Folder = folder ?? "avatars",
                    Transformation = new Transformation()
                        .Width(400)
                        .Height(400)
                        .Crop("fill")
                        .Gravity("face")
                        .Quality("auto")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return new FileUploadResult
                    {
                        Success = false,
                        Error = uploadResult.Error.Message
                    };
                }

                return new FileUploadResult
                {
                    Success = true,
                    Url = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId
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

        public async Task<bool> DeleteAsync(string fileIdentifier)
        {
            try
            {
                var deleteParams = new DeletionParams(fileIdentifier);
                var result = await _cloudinary.DestroyAsync(deleteParams);
                return result.Result == "ok";
            }
            catch
            {
                return false;
            }
        }
    }
}
