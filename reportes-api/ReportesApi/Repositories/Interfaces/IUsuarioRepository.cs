using ReportesApi.Models;

namespace ReportesApi.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario?> GetByCorreoAsync(string correo);
    Task<bool> ExistsByCorreoAsync(string correo);
    Task<Rol?> GetRolByNombreAsync(string nombre);
    Task AddAsync(Usuario usuario);
    Task SaveChangesAsync();
}