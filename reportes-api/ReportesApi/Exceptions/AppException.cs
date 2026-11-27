namespace ReportesApi.Exceptions;

/// <summary>
/// Excepción de negocio. Los servicios la lanzan y el middleware la convierte
/// en una respuesta HTTP con el formato estándar.
/// </summary>
public class AppException : Exception
{
    public int StatusCode { get; }
    public IEnumerable<string> Errors { get; }

    public AppException(int statusCode, string message, IEnumerable<string>? errors = null) : base(message)
    {
        StatusCode = statusCode;
        Errors = errors ?? Array.Empty<string>();
    }

    public static AppException BadRequest(string msg, params string[] errors) => new(400, msg, errors);
    public static AppException Unauthorized(string msg) => new(401, msg);
    public static AppException Forbidden(string msg) => new(403, msg);
    public static AppException NotFound(string msg) => new(404, msg);
    public static AppException Conflict(string msg, params string[] errors) => new(409, msg, errors);
}