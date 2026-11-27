using ReportesApi.DTOs;
using ReportesApi.Exceptions;
using ReportesApi.Models;
using ReportesApi.Repositories.Interfaces;
using ReportesApi.Services.Interfaces;

namespace ReportesApi.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repo;

    public CategoriaService(ICategoriaRepository repo) => _repo = repo;

    public async Task<List<CategoriaResponseDto>> GetAllAsync(bool incluirInactivas)
    {
        var categorias = await _repo.GetAllAsync(incluirInactivas);
        return categorias.Select(Map).ToList();
    }

    public async Task<CategoriaResponseDto> GetByIdAsync(int id)
    {
        var categoria = await _repo.GetByIdAsync(id)
                        ?? throw AppException.NotFound($"No existe la categoría {id}");
        return Map(categoria);
    }

    public async Task<CategoriaResponseDto> CreateAsync(CategoriaCreateDto dto)
    {
        var nombre = dto.Nombre.Trim();
        if (await _repo.ExistsByNombreAsync(nombre))
            throw AppException.Conflict("Ya existe una categoría con ese nombre");

        var categoria = new Categoria
        {
            Nombre = nombre,
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
            Activa = true
        };
        await _repo.AddAsync(categoria);
        await _repo.SaveChangesAsync();
        return Map(categoria);
    }

    public async Task<CategoriaResponseDto> UpdateAsync(int id, CategoriaUpdateDto dto)
    {
        var categoria = await _repo.GetByIdAsync(id)
                        ?? throw AppException.NotFound($"No existe la categoría {id}");

        var nombre = dto.Nombre.Trim();
        if (await _repo.ExistsByNombreAsync(nombre, excluirId: id))
            throw AppException.Conflict("Ya existe una categoría con ese nombre");

        categoria.Nombre = nombre;
        categoria.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();
        categoria.Activa = dto.Activa; // false = desactivar (RF-12)

        await _repo.SaveChangesAsync();
        return Map(categoria);
    }

    private static CategoriaResponseDto Map(Categoria c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        Descripcion = c.Descripcion,
        Activa = c.Activa
    };
}