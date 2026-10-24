using Classwork_17_10_2026.DTO;

namespace Classwork_17_10_2026.Services
{
    public interface IFileService
    {
        Task<FileResponseDto> SaveFileAsync(IFormFile file, string subFolder);
        void DeleteFile(string relativePath);
    }
}