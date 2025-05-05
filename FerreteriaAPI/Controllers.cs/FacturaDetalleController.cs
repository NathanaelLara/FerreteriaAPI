using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;
using FerreteriaAPI.Models;
using FerreteriaAPI.DTOs;


namespace FerreteriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaDetalleController : ControllerBase
    {
        private readonly FerreteriaDbContext _context;

        public FacturaDetalleController(FerreteriaDbContext context)
        {
            _context = context;
        }

        // GET: api/facturadetalle/factura/5
        [HttpGet("factura/{facturaId}")]
        public async Task<ActionResult<IEnumerable<FacturaDetalle>>> GetDetallesDeFactura(int facturaId)
        {
            return await _context.FacturaDetalles
                .Include(fd => fd.Item)
                .Where(fd => fd.FacturaId == facturaId)
                .ToListAsync();
        }

        // POST: api/facturadetalle
        [HttpPost]
        public async Task<ActionResult<FacturaDetalle>> PostDetalle(FacturaDetalleCreateDto dto)
        {
            var item = await _context.Items.FindAsync(dto.ItemId);
            if (item == null)
                return NotFound("Item no encontrado.");

            if (item.StockDisponible < dto.Cantidad)
                return BadRequest("Stock insuficiente para este item.");

            var detalle = new FacturaDetalle
            {
                FacturaId = dto.FacturaId,
                ItemId = dto.ItemId,
                Cantidad = dto.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            };

            item.StockDisponible -= dto.Cantidad;

            _context.FacturaDetalles.Add(detalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetallesDeFactura), new { facturaId = detalle.FacturaId }, detalle);
        }

        // PUT: api/facturadetalle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalle(int id, FacturaDetalle detalle)
        {
            if (id != detalle.Id)
                return BadRequest();

            var existente = await _context.FacturaDetalles
                .Include(d => d.Item)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (existente == null)
                return NotFound();

            var item = existente.Item;
            int diferencia = detalle.Cantidad - existente.Cantidad;

            if (item.StockDisponible < diferencia)
                return BadRequest("No hay suficiente stock para aumentar la cantidad.");

            existente.Cantidad = detalle.Cantidad;
            existente.PrecioUnitario = detalle.PrecioUnitario;

            item.StockDisponible -= diferencia;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/facturadetalle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalle(int id)
        {
            var detalle = await _context.FacturaDetalles
                .Include(d => d.Item)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (detalle == null)
                return NotFound();

            // Reponer stock
            detalle.Item.StockDisponible += detalle.Cantidad;

            _context.FacturaDetalles.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
