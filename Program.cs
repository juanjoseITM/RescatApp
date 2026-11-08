using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RescatApp.Interfaces;
using RescatApp.Middleware;
using RescatApp.Repositories;
using RescatApp.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Controladores (los enums viajan en el JSON como texto: "Adoptante", no 1)
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// CORS: solo los orígenes del frontend listados en appsettings.json pueden llamar la API desde el navegador
var origenesPermitidos = builder.Configuration.GetSection("Cors:OrigenesPermitidos").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(origenesPermitidos)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// 2. Inyección de Dependencias (SOLID)
builder.Services.AddScoped<IMascotasRepository, MascotasRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<ISolicitudesAdopcionRepository, SolicitudesAdopcionRepository>();

// Servicios de Lógica de Negocio y Seguridad
builder.Services.AddScoped<MascotasServices>();
builder.Services.AddScoped<SeguimientosRepository>();
builder.Services.AddScoped<SeguimientosServices>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<RefugioRepository>();
builder.Services.AddScoped<RefugioService>();

builder.Services.AddScoped<IUsuariosServices, UsuariosServices>();

// Manejo global de excepciones
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "RescatApp.xml"));

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

// Primero en el pipeline para atrapar las excepciones de todo lo que sigue
app.UseExceptionHandler();

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

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();