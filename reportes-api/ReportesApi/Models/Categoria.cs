namespace ReportesApi.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activa { get; set; } = true;

    public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
}