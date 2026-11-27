using Microsoft.EntityFrameworkCore;
using ReportesApi.Data;
using ReportesApi.Models;
using ReportesApi.Repositories.Interfaces;

namespace ReportesApi.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _db;

    public CategoriaRepository(AppDbContext db) => _db = db;

    public Task<List<Categoria>> GetAllAsync(bool incluirInactivas)
    {
        var query = _db.Categorias.AsNoTracking().AsQueryable();
        if (!incluirInactivas) query = query.Where(c => c.Activa);
        return query.OrderBy(c => c.Nombre).ToListAsync();
    }

    public Task<Categoria?> GetByIdAsync(int id) =>
        _db.Categorias.FirstOrDefaultAsync(c => c.Id == id);

    public Task<bool> ExistsByNombreAsync(string nombre, int? excluirId = null) =>
        _db.Categorias.AnyAsync(c => c.Nombre == nombre && (excluirId == null || c.Id != excluirId));

    public async Task AddAsync(Categoria categoria) => await _db.Categorias.AddAsync(categoria);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}