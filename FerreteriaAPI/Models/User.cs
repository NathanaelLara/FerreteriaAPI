using System;
using System.Collections.Generic;

namespace FerreteriaAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // En producción usarías un hash
}
