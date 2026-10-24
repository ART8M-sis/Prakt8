using Classwork_17_10_2026.DTO;

namespace Classwork_17_10_2026.Services
{
    public class FileService : IFileService
    {
        private const string UploadsFolder = "uploads";
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<FileResponseDto> SaveFileAsync(IFormFile file, string subFolder)
        {
            var webRootPath = _env.WebRootPath
                ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var uploadsDir = Path.Combine(webRootPath, UploadsFolder, subFolder);
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsDir, uniqueFileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new FileResponseDto
            {
                RelativePath = $"/{UploadsFolder}/{subFolder}/{uniqueFileName}",
                FileName = uniqueFileName,
                FileSize = file.Length
            };
        }

        public void DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;

            var webRootPath = _env.WebRootPath
                ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var clean = relativePath.TrimStart('/', '\\')
                .Replace('/', Path.DirectorySeparatorChar);

            var fullPath = Path.GetFullPath(Path.Combine(webRootPath, clean));

            if (!fullPath.StartsWith(webRootPath, StringComparison.OrdinalIgnoreCase))
                return;

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}