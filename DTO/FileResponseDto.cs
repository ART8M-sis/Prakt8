namespace Classwork_17_10_2026.DTO
{
    public class FileResponseDto
    {
        public string RelativePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }
}