using Microsoft.EntityFrameworkCore;
using ReportesApi.Data;
using ReportesApi.Models;
using ReportesApi.Repositories.Interfaces;

namespace ReportesApi.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _db;

    public UsuarioRepository(AppDbContext db) => _db = db;

    public Task<Usuario?> GetByIdAsync(int id) =>
        _db.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);

    public Task<Usuario?> GetByCorreoAsync(string correo) =>
        _db.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Correo == correo);

    public Task<bool> ExistsByCorreoAsync(string correo) =>
        _db.Usuarios.AnyAsync(u => u.Correo == correo);

    public Task<Rol?> GetRolByNombreAsync(string nombre) =>
        _db.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);

    public async Task AddAsync(Usuario usuario) => await _db.Usuarios.AddAsync(usuario);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}