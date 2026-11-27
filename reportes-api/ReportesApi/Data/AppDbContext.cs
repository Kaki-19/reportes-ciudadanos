using Microsoft.EntityFrameworkCore;
using ReportesApi.Models;

namespace ReportesApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Reporte> Reportes => Set<Reporte>();
    public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------- Roles ----------
        modelBuilder.Entity<Rol>(e =>
        {
            e.ToTable("Roles");
            e.Property(r => r.Nombre).HasMaxLength(30).IsRequired();
            e.HasIndex(r => r.Nombre).IsUnique();
            e.HasData(
                new Rol { Id = 1, Nombre = Rol.Ciudadano },
                new Rol { Id = 2, Nombre = Rol.Administrador });
        });

        // ---------- Usuarios ----------
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("Usuarios");
            e.Property(u => u.Nombre).HasMaxLength(100).IsRequired();
            e.Property(u => u.Correo).HasMaxLength(150).IsRequired();
            e.HasIndex(u => u.Correo).IsUnique();
            e.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
            e.Property(u => u.FechaRegistro).HasDefaultValueSql("SYSDATETIME()");
            e.Property(u => u.Activo).HasDefaultValue(true).HasSentinel(true);
            e.HasOne(u => u.Rol)
             .WithMany(r => r.Usuarios)
             .HasForeignKey(u => u.RolId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Categorias ----------
        modelBuilder.Entity<Categoria>(e =>
        {
            e.ToTable("Categorias");
            e.Property(c => c.Nombre).HasMaxLength(60).IsRequired();
            e.HasIndex(c => c.Nombre).IsUnique();
            e.Property(c => c.Descripcion).HasMaxLength(200);
            e.Property(c => c.Activa).HasDefaultValue(true).HasSentinel(true);
            e.HasData(
                new Categoria { Id = 1, Nombre = "Baches", Descripcion = "Baches y daños en el pavimento", Activa = true },
                new Categoria { Id = 2, Nombre = "Alumbrado", Descripcion = "Luminarias apagadas o dañadas", Activa = true },
                new Categoria { Id = 3, Nombre = "Fugas de agua", Descripcion = "Fugas en la red de agua potable", Activa = true },
                new Categoria { Id = 4, Nombre = "Basura", Descripcion = "Basura acumulada en la vía pública", Activa = true });
        });

        // ---------- Estados (catálogo fijo, Id sin IDENTITY) ----------
        modelBuilder.Entity<Estado>(e =>
        {
            e.ToTable("Estados");
            e.Property(s => s.Id).ValueGeneratedNever();
            e.Property(s => s.Nombre).HasMaxLength(30).IsRequired();
            e.HasIndex(s => s.Nombre).IsUnique();
            e.Property(s => s.Color).HasMaxLength(7).IsUnicode(false).IsRequired();
            e.HasData(
                new Estado { Id = Estado.Pendiente, Nombre = "Pendiente", Color = "#F59E0B" },
                new Estado { Id = Estado.EnRevision, Nombre = "En revisión", Color = "#3B82F6" },
                new Estado { Id = Estado.EnProceso, Nombre = "En proceso", Color = "#8B5CF6" },
                new Estado { Id = Estado.Resuelto, Nombre = "Resuelto", Color = "#10B981" },
                new Estado { Id = Estado.Rechazado, Nombre = "Rechazado", Color = "#EF4444" });
        });

        // ---------- Reportes ----------
        modelBuilder.Entity<Reporte>(e =>
        {
            e.ToTable("Reportes", t =>
            {
                t.HasCheckConstraint("CK_Reportes_Latitud", "[Latitud] BETWEEN -90 AND 90");
                t.HasCheckConstraint("CK_Reportes_Longitud", "[Longitud] BETWEEN -180 AND 180");
            });
            e.Property(r => r.Folio).HasMaxLength(20).IsUnicode(false).IsRequired();
            e.HasIndex(r => r.Folio).IsUnique();
            e.Property(r => r.Titulo).HasMaxLength(120).IsRequired();
            e.Property(r => r.Descripcion).HasMaxLength(1000).IsRequired();
            e.Property(r => r.EstadoId).HasDefaultValue(Estado.Pendiente);
            e.Property(r => r.Latitud).HasPrecision(9, 6);
            e.Property(r => r.Longitud).HasPrecision(9, 6);
            e.Property(r => r.Direccion).HasMaxLength(250);
            e.Property(r => r.FotoUrl).HasMaxLength(300).IsRequired();
            e.Property(r => r.FechaCreacion).HasDefaultValueSql("SYSDATETIME()");

            e.HasOne(r => r.Usuario).WithMany(u => u.Reportes)
             .HasForeignKey(r => r.UsuarioId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(r => r.Categoria).WithMany(c => c.Reportes)
             .HasForeignKey(r => r.CategoriaId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(r => r.Estado).WithMany()
             .HasForeignKey(r => r.EstadoId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- HistorialEstados ----------
        modelBuilder.Entity<HistorialEstado>(e =>
        {
            e.ToTable("HistorialEstados");
            e.Property(h => h.Comentario).HasMaxLength(500);
            e.Property(h => h.Fecha).HasDefaultValueSql("SYSDATETIME()");

            // Único borrado en cascada: al eliminar un reporte se borra su historial
            e.HasOne(h => h.Reporte).WithMany(r => r.Historial)
             .HasForeignKey(h => h.ReporteId).OnDelete(DeleteBehavior.Cascade);

            // El resto en Restrict para evitar "multiple cascade paths" en SQL Server
            e.HasOne(h => h.EstadoAnterior).WithMany()
             .HasForeignKey(h => h.EstadoAnteriorId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(h => h.EstadoNuevo).WithMany()
             .HasForeignKey(h => h.EstadoNuevoId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(h => h.Usuario).WithMany()
             .HasForeignKey(h => h.UsuarioId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}