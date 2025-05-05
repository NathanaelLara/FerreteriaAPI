using System;
using System.Collections.Generic;

namespace FerreteriaAPI.Models;
public class FacturaDetalle
{
    public int Id { get; set; }
    public int FacturaId { get; set; }
    public Factura Factura { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
