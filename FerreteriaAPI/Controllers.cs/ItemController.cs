using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;
using FerreteriaAPI.Models;

namespace FerreteriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly FerreteriaDbContext _context;

        public ItemController(FerreteriaDbContext context)
        {
            _context = context;
        }

        // GET: api/item
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        // GET: api/item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();

            return item;
        }

        // POST: api/item
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        // PUT: api/item/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Items.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/item/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
