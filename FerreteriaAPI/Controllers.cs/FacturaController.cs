using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;
using FerreteriaAPI.Models;
using FerreteriaAPI.DTOs;


namespace FerreteriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly FerreteriaDbContext _context;

        public FacturaController(FerreteriaDbContext context)
        {
            _context = context;
        }

        // GET: api/factura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas()
        {
            return await _context.Facturas
                .Include(f => f.Empleado)
                .Where(f => !f.EsAnulada)
                .ToListAsync();
        }

        // GET: api/factura/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Empleado)
                .Include(f => f.Detalles)
                    .ThenInclude(d => d.Item)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null)
                return NotFound();

            return factura;
        }

        // POST: api/factura
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(FacturaCreateDto dto)
        {
            var factura = new Factura
            {
            EmpleadoId = dto.EmpleadoId
            };

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFactura), new { id = factura.Id }, factura);
        }

        // PUT: api/factura/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, Factura factura)
        {
            if (id != factura.Id)
                return BadRequest();

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Facturas.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE (soft): api/factura/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> AnularFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
                return NotFound();

            factura.EsAnulada = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
