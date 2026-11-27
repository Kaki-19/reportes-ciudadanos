namespace ReportesApi.Models;

// La entidad se crea desde esta semana para que la migración inicial
// genere toda la base de datos. Su CRUD se implementa en la semana 3.
public class Reporte
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public int EstadoId { get; set; } = Estado.Pendiente;
    public Estado? Estado { get; set; }

    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public string? Direccion { get; set; }
    public string FotoUrl { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }

    public ICollection<HistorialEstado> Historial { get; set; } = new List<HistorialEstado>();
}