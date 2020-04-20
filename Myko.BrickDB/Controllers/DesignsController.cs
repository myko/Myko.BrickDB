using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Myko.BrickDB;

namespace Myko.BrickDB.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DesignsController : ControllerBase
    {
        private readonly BrickDbContext _context;

        public DesignsController(BrickDbContext context)
        {
            _context = context;
        }

        // GET: api/Designs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Design>>> GetDesigns()
        {
            return await _context.Designs.ToListAsync();
        }

        // GET: api/Designs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Design>> GetDesign(string id)
        {
            var design = await _context.Designs.FindAsync(id);

            if (design == null)
            {
                return NotFound();
            }

            return design;
        }

        // PUT: api/Designs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesign(string id, Design design)
        {
            if (id != design.DesignId)
            {
                return BadRequest();
            }

            _context.Entry(design).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DesignExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Designs
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Design>> PostDesign(Design design)
        {
            _context.Designs.Add(design);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DesignExists(design.DesignId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDesign", new { id = design.DesignId }, design);
        }

        // DELETE: api/Designs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Design>> DeleteDesign(string id)
        {
            var design = await _context.Designs.FindAsync(id);
            if (design == null)
            {
                return NotFound();
            }

            _context.Designs.Remove(design);
            await _context.SaveChangesAsync();

            return design;
        }

        private bool DesignExists(string id)
        {
            return _context.Designs.Any(e => e.DesignId == id);
        }
    }
}
