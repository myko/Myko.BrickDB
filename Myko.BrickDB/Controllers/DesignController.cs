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
    [Route("api/v1/designs")]
    [ApiController]
    public class DesignController : ControllerBase
    {
        private readonly BrickDbContext _context;

        public DesignController(BrickDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Design>>> GetDesigns()
        {
            return await _context.Designs.ToListAsync();
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesign(string id, Design design)
        {
            if (colorPut.Description == null)
                throw new ArgumentException();

            var color = await _context.Colors.FindAsync(id);

            if (color == null)
                color = new Color(id, colorPut.Description);
            else
                color.Description = colorPut.Description;

            _context.Colors.Add(color);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColor), new { id = color.ColorId }, color);
        }

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

    public class DesignPut
    {

    }
}
