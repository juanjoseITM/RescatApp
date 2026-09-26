using Microsoft.EntityFrameworkCore;
using RescatApp.Repositories;
using RescatApp.Services; //Service

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración de OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();

// Agrega Swagger UI interactivo
builder.Services.AddSwaggerGen(); 

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionSQL")));

//Registro de Inyección de Dependencias para el módulo Mascotas
builder.Services.AddScoped<MascotasRepository>();
builder.Services.AddScoped<MascotasServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Habilita la interfaz visual de Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
