namespace ReportesApi.Models;

public class Rol
{
    public const string Ciudadano = "Ciudadano";
    public const string Administrador = "Administrador";

    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}