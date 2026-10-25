using System.Text.Json;

namespace FileAvatar.Logging;

public interface IFileRequestLogger
{
    Task LogAsync(RequestLogEntry entry);
}

public class FileRequestLogger : IFileRequestLogger
{
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private readonly string _filePath;

    public FileRequestLogger(IWebHostEnvironment env)
    {
        var dir = Path.Combine(env.ContentRootPath, "logs");
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "requests.log");
    }

    public async Task LogAsync(RequestLogEntry entry)
    {
        var line = JsonSerializer.Serialize(entry,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            + Environment.NewLine;

        await _lock.WaitAsync();
        try { await File.AppendAllTextAsync(_filePath, line); }
        catch { }
        finally { _lock.Release(); }
    }
}