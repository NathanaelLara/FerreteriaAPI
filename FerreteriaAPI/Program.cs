using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;
using FerreteriaAPI.Models;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FerreteriaDbContext>(options =>
    options.UseSqlServer("Server=localhost; Database=FerreteriaDB;User Id=Sa;Password=Sa123456;TrustServerCertificate=True"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // 🔥 Esto es lo que registra los endpoints de tus controladores

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
}

app.Run();
