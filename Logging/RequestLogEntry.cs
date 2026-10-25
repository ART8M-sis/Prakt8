namespace FileAvatar.Logging;

public class RequestLogEntry
{
    public DateTime Timestamp { get; set; }
    public string Method { get; set; } = "";
    public string Path { get; set; } = "";
    public int StatusCode { get; set; }
    public long DurationMs { get; set; }
    public string Message { get; set; } = "";
}