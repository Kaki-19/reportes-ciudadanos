using ReportesApi.DTOs;
using ReportesApi.Exceptions;
using ReportesApi.Models;
using ReportesApi.Repositories.Interfaces;
using ReportesApi.Services.Interfaces;

namespace ReportesApi.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ITokenService _tokens;

    public AuthService(IUsuarioRepository usuarios, ITokenService tokens)
    {
        _usuarios = usuarios;
        _tokens = tokens;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var correo = NormalizarCorreo(dto.Correo);

        if (await _usuarios.ExistsByCorreoAsync(correo))
            throw AppException.Conflict("El correo ya está registrado");

        // Todo registro público es Ciudadano; los administradores se crean con el seeder
        var rol = await _usuarios.GetRolByNombreAsync(Rol.Ciudadano)
                  ?? throw new InvalidOperationException("No existe el rol Ciudadano. ¿Se aplicó la migración?");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Correo = correo,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // RNF-01
            RolId = rol.Id,
            Rol = rol,
            FechaRegistro = DateTime.Now,
            Activo = true
        };

        await _usuarios.AddAsync(usuario);
        await _usuarios.SaveChangesAsync();

        return CrearRespuesta(usuario);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarios.GetByCorreoAsync(NormalizarCorreo(dto.Correo));

        // Mismo mensaje si no existe o si la contraseña es incorrecta (no revela qué correos existen)
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            throw AppException.Unauthorized("Correo o contraseña incorrectos");

        if (!usuario.Activo)
            throw AppException.Forbidden("La cuenta está desactivada");

        return CrearRespuesta(usuario);
    }

    public async Task<UsuarioDto> GetMeAsync(int usuarioId)
    {
        var usuario = await _usuarios.GetByIdAsync(usuarioId)
                      ?? throw AppException.NotFound("Usuario no encontrado");
        return MapUsuario(usuario);
    }

    private AuthResponseDto CrearRespuesta(Usuario usuario)
    {
        var (token, expira) = _tokens.GenerarToken(usuario);
        return new AuthResponseDto { Token = token, ExpiraEn = expira, Usuario = MapUsuario(usuario) };
    }

    private static UsuarioDto MapUsuario(Usuario u) => new()
    {
        Id = u.Id,
        Nombre = u.Nombre,
        Correo = u.Correo,
        Rol = u.Rol?.Nombre ?? string.Empty
    };

    private static string NormalizarCorreo(string correo) => correo.Trim().ToLowerInvariant();
}