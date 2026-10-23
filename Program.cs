using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RescatApp.Interfaces;
using RescatApp.Repositories;
using RescatApp.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Controladores
builder.Services.AddControllers();

// 2. Inyección de Dependencias (SOLID)
builder.Services.AddScoped<IMascotasRepository, MascotasRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<ISolicitudesAdopcionRepository, SolicitudesAdopcionRepository>();

// Servicios de Lógica de Negocio y Seguridad
builder.Services.AddScoped<MascotasServices>();
builder.Services.AddScoped<SeguimientosRepository>();
builder.Services.AddScoped<SeguimientosServices>();
builder.Services.AddScoped<JwtService>();

// Módulo de Notificaciones
builder.Services.AddScoped<NotificacionesRepository>();
builder.Services.AddScoped<NotificacionesServices>();

// 3. Configuración de Autenticación JWT
var jwtKey = builder.Configuration["Jwt:Key"] 
    ?? throw new InvalidOperationException("La clave JWT no está configurada en appsettings.json.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 4. Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "RescatApp API", 
        Version = "v1" 
    });

    // Definición directa del esquema Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingresa el token JWT en el formato: Bearer {tu_token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Requerimiento de seguridad pasado como delegado Func<OpenApiDocument, OpenApiSecurityRequirement>
    options.AddSecurityRequirement((doc) =>
    {
        var requirement = new OpenApiSecurityRequirement();
        var schemeReference = new OpenApiSecuritySchemeReference("Bearer");
        requirement.Add(schemeReference, new List<string>());
        return requirement;
    });
});

var app = builder.Build();

// 5. Middleware HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RescatApp API v1");
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();