using System.Threading.RateLimiting;
using Amazon.S3;

using Core.Infrastructure;

using Identity.Infrastructure;
using Identity.Presentation;

using Logistics.Infrastructure;
using Logistics.Presentation;

using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

using Payment.Infrastructure;
using Payment.Presentation;

using Registration.Application.Interfaces;
using Registration.Infrastructure;
using Registration.Presentation;

using Web.API.Services;

// Load .env file from the root BEFORE creating the builder, así sus variables
// (incl. Storage__* y ASPNETCORE_URLS) son tomadas por el proveedor de variables
// de entorno de la configuración y por el host.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables
var pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
var pgDb = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "gestor_eventos";
var pgUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "user";
var pgPass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "password";

var envConnectionString = $"Host={pgHost};Port={pgPort};Database={pgDb};Username={pgUser};Password={pgPass};";

if (builder.Environment.IsDevelopment())
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = envConnectionString;
    builder.Configuration["ConnectionStrings:NeonPostgres"] = envConnectionString;
}

// Add services to the container.

builder.Services.AddHttpContextAccessor();

// Registra los controladores de todos los módulos que se añaden.
builder.Services.AddControllers();

// Configurar Amazon S3 / MinIO
var minioUser = Environment.GetEnvironmentVariable("MINIO_USER") ?? "minioadmin";
var minioPass = Environment.GetEnvironmentVariable("MINIO_PASSWORD") ?? "minioadmin";

// Endpoint interno que usa el SDK para subir objetos. El host público que queda
// embebido en las URLs devueltas al cliente se configura aparte en "Storage:PublicBaseUrl".
var minioServiceUrl = builder.Configuration["Storage:ServiceUrl"] ?? "http://localhost:9000";

var s3Config = new AmazonS3Config
{
    ServiceURL = minioServiceUrl,
    ForcePathStyle = true // Requerido para MinIO
};

builder.Services.AddSingleton<IAmazonS3>(sp => new AmazonS3Client(minioUser, minioPass, s3Config));

// Registra todas las dependencias del módulo de Eventos (Application, Infrastructure, etc.)

builder.Services.AddCoreInfrastructure(builder.Configuration);

builder.Services.AddLogisticsInfrastructure(builder.Configuration);
builder.Services.AddLogisticsPresentation();

builder.Services.AddRegistrationModule(builder.Configuration);

builder.Services.AddPaymentInfrastructure(builder.Configuration);
builder.Services.AddPaymentPresentation();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Identity Module
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddIdentityPresentation();

builder.Services.AddRegistrationPresentation();

// Adaptador que envía el QR por correo al aceptar una inscripción (usa los
// servicios de Logistics sin acoplar el módulo Registration con Logistics).
builder.Services.AddScoped<IAcceptanceNotifier, ParticipantAcceptanceNotifier>();

// Adaptador que expone la URL del comprobante (módulo Payment) al listado de
// inscripciones pendientes (módulo Registration) sin acoplar ambos módulos.
builder.Services.AddScoped<IReceiptUrlProvider, PaymentReceiptUrlProvider>();

builder.Services.AddHealthChecks();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// 1. Configurar Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    // Política para el registro público y login (5 peticiones por minuto por IP)
    options.AddFixedWindowLimiter("PublicEndpointsPolicy", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0; // Rechazar inmediatamente si se pasa el límite
    });

    // Devolver 429 Too Many Requests cuando se excede el límite
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];

    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// El middleware de CORS debe ir lo más arriba posible
app.UseCors();

// Sirve los comprobantes subidos desde wwwroot/receipts.
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// 2. Usar el middleware de Rate Limiting
app.UseRateLimiter();

// These middlewares must be added before MapControllers to secure the endpoints
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();