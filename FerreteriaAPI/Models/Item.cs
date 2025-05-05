using System;
using System.Collections.Generic;

namespace FerreteriaAPI.Models;
public class Item
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioUnitario { get; set; }
    public int StockDisponible { get; set; }
}
