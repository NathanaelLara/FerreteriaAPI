using System;
using System.Collections.Generic;

namespace FerreteriaAPI.Models;
public class Empleado
{
    public int Id { get; set; }
    public required string Nombre { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
}
