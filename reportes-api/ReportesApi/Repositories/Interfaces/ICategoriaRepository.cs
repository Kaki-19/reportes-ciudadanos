using ReportesApi.Models;

namespace ReportesApi.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Task<List<Categoria>> GetAllAsync(bool incluirInactivas);
    Task<Categoria?> GetByIdAsync(int id);
    Task<bool> ExistsByNombreAsync(string nombre, int? excluirId = null);
    Task AddAsync(Categoria categoria);
    Task SaveChangesAsync();
}