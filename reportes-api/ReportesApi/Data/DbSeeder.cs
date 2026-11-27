using Microsoft.EntityFrameworkCore;
using ReportesApi.Models;

namespace ReportesApi.Data;

/// <summary>
/// Crea el primer administrador al iniciar la API si todavía no existe.
/// El correo y la contraseña se leen de la configuración (AdminSeed:Correo / AdminSeed:Password),
/// así la contraseña no queda escrita en el código ni en la migración.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAdminAsync(AppDbContext db, IConfiguration config, ILogger logger)
    {
        var correo = config["AdminSeed:Correo"];
        var password = config["AdminSeed:Password"];
        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("AdminSeed no configurado: no se creó el usuario administrador.");
            return;
        }

        correo = correo.Trim().ToLowerInvariant();
        if (await db.Usuarios.AnyAsync(u => u.Correo == correo)) return;

        var rolAdmin = await db.Roles.FirstAsync(r => r.Nombre == Rol.Administrador);
        db.Usuarios.Add(new Usuario
        {
            Nombre = "Administrador",
            Correo = correo,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            RolId = rolAdmin.Id,
            FechaRegistro = DateTime.Now,
            Activo = true
        });
        await db.SaveChangesAsync();
        logger.LogInformation("Usuario administrador {Correo} creado.", correo);
    }
}