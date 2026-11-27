using System.ComponentModel.DataAnnotations;

namespace ReportesApi.DTOs;

public class CategoriaCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 60 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "La descripción admite máximo 200 caracteres")]
    public string? Descripcion { get; set; }
}

public class CategoriaUpdateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 60 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "La descripción admite máximo 200 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>false = desactivar la categoría (no se borra para no romper reportes existentes)</summary>
    public bool Activa { get; set; } = true;
}

public class CategoriaResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activa { get; set; }
}