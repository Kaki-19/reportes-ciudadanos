namespace ReportesApi.DTOs;

/// <summary>Formato estándar de error definido en la arquitectura (sección 2.3).</summary>
public class ErrorResponseDto
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

    public ErrorResponseDto() { }

    public ErrorResponseDto(string message, IEnumerable<string>? errors = null)
    {
        Message = message;
        Errors = errors ?? Array.Empty<string>();
    }
}