using System.ComponentModel.DataAnnotations;

namespace CarritoComprasApi.Dtos;

public class AgregarItemSolicitud
{
    [Range(1, int.MaxValue)]
    public int ProductoId { get; init; }

    [Range(1, 99)]
    public int Cantidad { get; init; }
}

public class CambiarCantidadSolicitud
{
    [Range(1, 99)]
    public int Cantidad { get; init; }
}

public record ItemCarritoRespuesta(
    int ProductoId, string Nombre, decimal PrecioUnitario, int Cantidad)
{
    public decimal Subtotal => PrecioUnitario * Cantidad;
}
