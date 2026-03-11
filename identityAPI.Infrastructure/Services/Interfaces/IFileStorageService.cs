using identityAPI.Infrastructure.Services.FileStorage;

namespace identityAPI.Infrastructure.Services.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Uploads a file and returns the public URL
        /// </summary>
        /// <param name="fileStream">File content stream</param>
        /// <param name="fileName">Original file name</param>
        /// <param name="folder">Optional folder/path for organization</param>
        /// <returns>Public URL of the uploaded file</returns>
        Task<FileUploadResult> UploadAsync(Stream fileStream, string fileName, string? folder = null);

        /// <summary>
        /// Deletes a file by its public ID or URL
        /// </summary>
        /// <param name="fileIdentifier">Public ID or URL of the file</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteAsync(string fileIdentifier);
    }
}
