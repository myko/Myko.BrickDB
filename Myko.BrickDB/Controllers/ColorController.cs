using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Myko.BrickDB.Controllers
{
    [Route("api/v1/colors")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly BrickDbContext _context;
        private readonly LinkGenerator _linkGenerator;

        public ColorController(BrickDbContext context, LinkGenerator linkGenerator)
        {
            _context = context;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorListView>>> GetColors()
        {
            return await _context.Colors
                .Select(x => new ColorListView { ColorId = x.ColorId, Description = x.Description })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColorView>> GetColor(string id)
        {
            var color = await _context.Colors
                .Select(x => new ColorView
                {
                    ColorId = x.ColorId,
                    Description = x.Description,
                })
                .SingleOrDefaultAsync(x => x.ColorId == id);

            if (color == null)
                return NotFound();

            return color;
        }

        [HttpGet("{id}/elements")]
        public async Task<ActionResult<IEnumerable<ColorElementView>>> GetColorElements(string id)
        {
            if (!await _context.Colors.AnyAsync(x => x.ColorId == id))
                return NotFound();

            var elements = await _context.Elements
                .Where(x => x.Color != null && x.Color.ColorId == id)
                .Select(x => new ColorElementView
                {
                    ElementId = x.ElementId,
                    Description = x.Description,
                })
                .ToListAsync();

            foreach (var element in elements)
            {
                element.Link = new Link(_linkGenerator.GetUriByAction(HttpContext, "GetDesign", "Design", values: new { id = element.ElementId }), "element", "GET");
            }

            return elements;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(string id, ColorPut colorPut)
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
        public async Task<ActionResult<Color>> DeleteColor(string id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
                return NotFound();

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return color;
        }
    }

    public class ColorView
    {
        public string? ColorId { get; set; }
        public string? Description { get; set; }
    }

    public class ColorElementView
    {
        public string? ElementId { get; set; }
        public string? Description { get; set; }
        public Link? Link { get; set; }
    }

    public class ColorListView
    {
        public string? ColorId { get; set; }
        public string? Description { get; set; }
    }

    public class ColorPut
    {
        [Required] public string? Description { get; set; }
    }
}
