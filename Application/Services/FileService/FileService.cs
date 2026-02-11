using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.FileService
{
    public class FileService : IFileService
    { private readonly IWebHostEnvironment _evn;
        public FileService(IWebHostEnvironment evn)
        {
            _evn = evn;
        }
        public async Task<string?> SaveFileAsync(IFormFile? file, string folderName)
        {
            if(file == null || file.Length == 0)
            {
                return null;
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var folderPath = Path.Combine(_evn.WebRootPath,folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath); 
            }
            var filePath = Path.Combine(folderPath,fileName);
            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine(folderName,fileName).Replace("\\","/");
        }
        public async void DeleteFile(string? filePath)
        {
            if(!string.IsNullOrEmpty(filePath)) return;
            var fullPath = Path.Combine(_evn.WebRootPath,filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}