using ReportesApi.Models;

namespace ReportesApi.Services.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiraEn) GenerarToken(Usuario usuario);
}