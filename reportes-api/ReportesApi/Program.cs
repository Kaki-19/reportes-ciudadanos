using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReportesApi.Data;
using ReportesApi.DTOs;
using ReportesApi.Middleware;
using ReportesApi.Repositories;
using ReportesApi.Repositories.Interfaces;
using ReportesApi.Services;
using ReportesApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
// User-secrets también cuando "dotnet ef" ejecuta el proyecto (ahí el entorno no es Development)
builder.Configuration.AddUserSecrets<Program>(optional: true);
var config = builder.Configuration;

// ---------- Base de datos ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// ---------- Inyección de dependencias (3 capas) ----------
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

// ---------- Controllers + formato estándar para errores de validación (400) ----------
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errores = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Valor inválido" : e.ErrorMessage);
        return new BadRequestObjectResult(new ErrorResponseDto("Datos inválidos", errores));
    };
});

// ---------- JWT ----------
var jwtKey = config["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32)
    throw new InvalidOperationException(
        "Falta Jwt:Key (mínimo 32 caracteres). Configúrala con: dotnet user-secrets set \"Jwt:Key\" \"<clave>\" " +
        "o con la variable de entorno Jwt__Key.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Usamos los nombres cortos de claims (sub, email, role) tal como se emiten
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = config["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = "sub",
            RoleClaimType = "role"
        };
        // 401 y 403 con el mismo formato JSON que el resto de errores
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async ctx =>
            {
                ctx.HandleResponse();
                await ExceptionHandlingMiddleware.WriteAsync(ctx.HttpContext, StatusCodes.Status401Unauthorized,
                    new ErrorResponseDto("No autenticado: token ausente, inválido o expirado"));
            },
            OnForbidden = async ctx =>
            {
                await ExceptionHandlingMiddleware.WriteAsync(ctx.HttpContext, StatusCodes.Status403Forbidden,
                    new ErrorResponseDto("No tienes permiso para realizar esta acción"));
            }
        };
    });
builder.Services.AddAuthorization();

// ---------- CORS ----------
builder.Services.AddCors(options =>
{
    options.AddPolicy("App", policy =>
    {
        var origenes = config.GetSection("Cors:Origenes").Get<string[]>() ?? Array.Empty<string>();
        if (builder.Environment.IsDevelopment() || origenes.Length == 0)
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        else
            policy.WithOrigins(origenes).AllowAnyHeader().AllowAnyMethod();
    });
});

// ---------- Swagger con botón "Authorize" para el JWT ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Reportes API",
        Version = "v1",
        Description = "API del sistema de reporte ciudadano de problemas urbanos"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pega solo el token (sin la palabra Bearer)"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ---------- Aplicar migraciones y crear admin al iniciar ----------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAdminAsync(db, config, logger);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // En desarrollo no redirigimos a HTTPS para que el celular (Expo Go) entre por http://IP:5000
    app.UseHttpsRedirection();
}

app.UseStaticFiles(); // wwwroot/uploads (imágenes, semana 3)
app.UseCors("App");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();