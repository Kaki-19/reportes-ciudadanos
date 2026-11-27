namespace ReportesApi.Models;

public class HistorialEstado
{
    public int Id { get; set; }

    public int ReporteId { get; set; }
    public Reporte? Reporte { get; set; }

    public int? EstadoAnteriorId { get; set; }
    public Estado? EstadoAnterior { get; set; }

    public int EstadoNuevoId { get; set; }
    public Estado? EstadoNuevo { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public string? Comentario { get; set; }
    public DateTime Fecha { get; set; }
}