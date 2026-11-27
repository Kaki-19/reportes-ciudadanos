using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ReportesApi.Exceptions;

namespace ReportesApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>Obtiene el Id del usuario desde el claim "sub" del JWT.</summary>
    public static int GetUsuarioId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return int.TryParse(sub, out var id) ? id : throw AppException.Unauthorized("Token inválido");
    }

    public static bool EsAdministrador(this ClaimsPrincipal user) => user.IsInRole(Models.Rol.Administrador);
}