using ReportesApi.DTOs;

namespace ReportesApi.Services.Interfaces;

public interface ICategoriaService
{
    Task<List<CategoriaResponseDto>> GetAllAsync(bool incluirInactivas);
    Task<CategoriaResponseDto> GetByIdAsync(int id);
    Task<CategoriaResponseDto> CreateAsync(CategoriaCreateDto dto);
    Task<CategoriaResponseDto> UpdateAsync(int id, CategoriaUpdateDto dto);
}