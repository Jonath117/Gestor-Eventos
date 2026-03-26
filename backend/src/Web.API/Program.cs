using Events.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Registra los controladores de todos los módulos que se añaden.
builder.Services.AddControllers();

// Registra todas las dependencias del módulo de Eventos (Application, Infrastructure, etc.)
builder.Services.AddEventsModule();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
