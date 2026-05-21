using System.Threading.RateLimiting;

using Core.Infrastructure;

using Identity.Infrastructure;
using Identity.Presentation;

using Logistics.Infrastructure;

using Microsoft.AspNetCore.RateLimiting;

using Payment.Infrastructure;

using Registration.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

// Registra los controladores de todos los módulos que se añaden.
builder.Services.AddControllers();

// Registra todas las dependencias del módulo de Eventos (Application, Infrastructure, etc.)

builder.Services.AddCoreInfrastructure(builder.Configuration);

builder.Services.AddLogisticsInfrastructure(builder.Configuration);

builder.Services.AddRegistrationInfrastructure(builder.Configuration);

builder.Services.AddPaymentInfrastructure(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Identity Module
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddIdentityPresentation();

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

app.Run();