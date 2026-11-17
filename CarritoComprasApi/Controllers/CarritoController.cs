using CarritoComprasApi.Data;
using CarritoComprasApi.Dtos;
using CarritoComprasApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CarritoComprasApi.Controllers;

[ApiController]
[Route("api/carrito")]
public class CarritoController : ControllerBase
{
    private readonly CarritoDbContext db;

    public CarritoController(CarritoDbContext db)
    {
        this.db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Consultar()
    {
        var items = await db.ItemsCarrito.AsNoTracking()
            .OrderBy(i => i.ProductoId)
            .Select(i => new ItemCarritoRespuesta(
                i.ProductoId, i.Producto.Nombre, i.Producto.Precio, i.Cantidad))
            .ToListAsync();

        return Ok(new { items, total = items.Sum(i => i.Subtotal) });
    }

    [HttpPost("items")]
    public async Task<IActionResult> Agregar([FromBody] AgregarItemSolicitud solicitud)
    {
        var producto = await db.Productos.FindAsync(solicitud.ProductoId);
        if (producto is null)
        {
            return NotFound(new { mensaje = "El producto no existe en el catálogo." });
        }

        var item = await db.ItemsCarrito.FindAsync(producto.Id);
        int cantidad = (item?.Cantidad ?? 0) + solicitud.Cantidad;

        if (cantidad > 99)
        {
            return BadRequest(new { mensaje = "La cantidad acumulada supera 99." });
        }

        if (item is null)
        {
            item = new ItemCarrito { ProductoId = producto.Id, Cantidad = cantidad };
            db.ItemsCarrito.Add(item);
        }
        else
        {
            item.Cantidad = cantidad;
        }

        return await GuardarAsync(Ok(new ItemCarritoRespuesta(
            producto.Id, producto.Nombre, producto.Precio, cantidad)));
    }

    [HttpPut("items/{productoId:int}")]
    public async Task<IActionResult> CambiarCantidad(
        int productoId, [FromBody] CambiarCantidadSolicitud solicitud)
    {
        var item = await db.ItemsCarrito.Include(i => i.Producto)
            .SingleOrDefaultAsync(i => i.ProductoId == productoId);

        if (item is null)
        {
            return NotFound(new { mensaje = "El producto no está en el carrito." });
        }

        item.Cantidad = solicitud.Cantidad;
        return await GuardarAsync(Ok(new ItemCarritoRespuesta(
            item.ProductoId, item.Producto.Nombre, item.Producto.Precio, item.Cantidad)));
    }

    [HttpDelete("items/{productoId:int}")]
    public async Task<IActionResult> Quitar(int productoId)
    {
        var item = await db.ItemsCarrito.FindAsync(productoId);

        if (item is null)
        {
            return NotFound(new { mensaje = "El producto no está en el carrito." });
        }

        db.ItemsCarrito.Remove(item);
        return await GuardarAsync(NoContent());
    }

    private async Task<IActionResult> GuardarAsync(IActionResult resultado)
    {
        try
        {
            await db.SaveChangesAsync();
            return resultado;
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { mensaje = "El carrito cambió. Consulta su estado e intenta de nuevo." });
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            return Conflict(new { mensaje = "El producto ya se agregó en otra solicitud. Consulta el carrito." });
        }
    }
}
