using System.Text.Json;
using ReportesApi.DTOs;
using ReportesApi.Exceptions;

namespace ReportesApi.Middleware;

/// <summary>
/// Captura cualquier excepción y responde siempre con { success, message, errors }.
/// Los errores 500 no exponen la traza interna (RNF de seguridad).
/// </summary>
public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            await WriteAsync(context, ex.StatusCode, new ErrorResponseDto(ex.Message, ex.Errors));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado en {Path}", context.Request.Path);
            await WriteAsync(context, StatusCodes.Status500InternalServerError,
                new ErrorResponseDto("Ocurrió un error inesperado. Intenta más tarde."));
        }
    }

    public static async Task WriteAsync(HttpContext context, int statusCode, ErrorResponseDto body)
    {
        if (context.Response.HasStarted) return;
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";
        await context.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOptions));
    }
}