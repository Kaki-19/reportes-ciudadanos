namespace ReportesApi.Models;

public class Estado
{
    // Ids fijos para usarlos en el flujo de estados (no son IDENTITY)
    public const int Pendiente = 1;
    public const int EnRevision = 2;
    public const int EnProceso = 3;
    public const int Resuelto = 4;
    public const int Rechazado = 5;

    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Color { get; set; } = "#000000";
}