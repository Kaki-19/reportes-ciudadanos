# Reportes API (ASP.NET Core 8 + EF Core + SQL Server)

Arquitectura en tres capas: Controllers → Services → Repositories.

## Requisitos
- .NET 8 SDK
- SQL Server Express o Developer
- Herramienta de EF: `dotnet tool install --global dotnet-ef --version 8.*`

## Primera vez

```bash
cd ReportesApi

# 1) Secretos (NO van al repositorio)
dotnet user-secrets set "Jwt:Key" "una-clave-larga-de-al-menos-32-caracteres-123456"
dotnet user-secrets set "AdminSeed:Password" "Admin12345"

# 2) Revisa la cadena de conexión en appsettings.json (servidor localhost\SQLEXPRESS)

# 3) Migración inicial y base de datos
dotnet ef migrations add InitialCreate
dotnet ef database update

# 4) Ejecutar
dotnet run --launch-profile ReportesApi
```

- Swagger: https://localhost:7000/swagger
- Desde el celular: `http://<IP-de-tu-PC>:5000/api` (permite el puerto 5000 en el firewall de Windows).
- Admin inicial: `admin@reportes.local` con la contraseña que pusiste en `AdminSeed:Password`.

## Endpoints de la semana 2

| Método | Ruta | Rol |
|---|---|---|
| POST | /api/auth/register | público |
| POST | /api/auth/login | público |
| GET | /api/auth/me | cualquiera con sesión |
| GET | /api/categorias | cualquiera con sesión |
| GET | /api/categorias/{id} | cualquiera con sesión |
| POST | /api/categorias | Administrador |
| PUT | /api/categorias/{id} | Administrador (enviar `"activa": false` para desactivar) |

Pruebas rápidas en `ReportesApi.http`.