namespace FerreteriaAPI.DTOs;

public class FacturaDetalleCreateDto
{
    public int FacturaId { get; set; }
    public int ItemId { get; set; }
    public int Cantidad { get; set; }
}
