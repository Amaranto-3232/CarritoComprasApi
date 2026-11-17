using CarritoComprasApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cadena = builder.Configuration.GetConnectionString("CarritoDb")
    ?? throw new InvalidOperationException("Falta la conexión CarritoDb.");

builder.Services.AddDbContext<CarritoDbContext>(options =>
    options.UseSqlServer(cadena));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
