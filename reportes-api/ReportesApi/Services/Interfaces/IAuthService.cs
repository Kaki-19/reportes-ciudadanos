using ReportesApi.DTOs;

namespace ReportesApi.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<UsuarioDto> GetMeAsync(int usuarioId);
}