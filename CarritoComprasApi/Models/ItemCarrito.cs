namespace CarritoComprasApi.Models;

public class ItemCarrito
{
    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;
    public int Cantidad { get; set; }
    public byte[] Version { get; set; } = null!;
}
