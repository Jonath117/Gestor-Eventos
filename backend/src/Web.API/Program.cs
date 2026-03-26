using Events.Infrastructure;
using Identity.Presentation;
using Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Registra los controladores de todos los módulos que se añaden.
builder.Services.AddControllers();

// Registra todas las dependencias del módulo de Eventos (Application, Infrastructure, etc.)
builder.Services.AddEventsModule();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Identity Module
// The JWT Bearer authentication is configured inside AddIdentityInfrastructure
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddIdentityModule(builder.Configuration);

builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// These middlewares must be added before MapControllers to secure the endpoints
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
