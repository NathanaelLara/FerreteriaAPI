using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;
using FerreteriaAPI.Models;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BCrypt.Net;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ferreteria API", Version = "v1" });

    // JWT Configuration
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token aquí: Bearer {tu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<FerreteriaDbContext>(options =>
    options.UseSqlServer("Server=localhost; Database=FerreteriaDB;User Id=Sa;Password=Sa123456;TrustServerCertificate=True"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddMemoryCache();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FerreteriaDbContext>();

    // Crear Empleados si no existen
    if (!context.Empleados.Any())
    {
        context.Empleados.AddRange(
            new Empleado { Nombre = "Juan Pérez", Cargo = "Cajero" },
            new Empleado { Nombre = "Ana Torres", Cargo = "Vendedor" }
        );
    }

    // Crear Items si no existen
    if (!context.Items.Any())
    {
        context.Items.AddRange(
            new Item { Nombre = "Martillo", PrecioUnitario = 250, StockDisponible = 15 },
            new Item { Nombre = "Destornillador", PrecioUnitario = 120, StockDisponible = 30 },
            new Item { Nombre = "Sierra", PrecioUnitario = 600, StockDisponible = 10 }
        );
    }

    context.SaveChanges();

   if (!context.Users.Any())
    {
        context.Users.Add(new User 
        {
        Username = "admin",
        Password = BCrypt.Net.BCrypt.HashPassword("1234")
        });
    }
        context.SaveChanges();
}

app.Run();
