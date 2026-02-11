using Microsoft.AspNetCore.Http;

namespace Application.Services.FileService
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(IFormFile? file, string folderName);
        void DeleteFile(string? filePath);
    }
}