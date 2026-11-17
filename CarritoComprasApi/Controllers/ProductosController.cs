using CarritoComprasApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarritoComprasApi.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly CarritoDbContext db;

    public ProductosController(CarritoDbContext db)
    {
        this.db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var productos = await db.Productos.AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync();
        return Ok(productos);
    }
}
