using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Abstractions;

namespace FerreteriaAPI.Models;
public class Factura
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int EmpleadoId { get; set; }
    public Empleado Empleado { get; set; } = null!;
    public bool EsAnulada { get; set; } = false;
    public List<FacturaDetalle> Detalles { get; set; } = new();
}
