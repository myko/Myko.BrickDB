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
    public class ColorController : BrickDbControllerBase<Color>
    {
        private readonly LinkGenerator _linkGenerator;

        public ColorController(BrickDbContext context, LinkGenerator linkGenerator)
            : base(context)
        {
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
        public Task<ActionResult<ColorView>> GetColor(string id)
        {
            return GetSingle(
                x => x.ColorId == id,
                x => new ColorView
                {
                    ColorId = x.ColorId,
                    Description = x.Description,
                });
        }

        [HttpGet("{id}/elements")]
        public async Task<ActionResult<IEnumerable<ElementLinkView>>> GetColorElements(string id)
        {
            if (!await _context.Colors.AnyAsync(x => x.ColorId == id))
                return NotFound();

            var elements = await _context.Elements
                .Where(x => x.Color != null && x.Color.ColorId == id)
                .Select(x => new ElementLinkView
                {
                    ElementId = x.ElementId,
                    Description = x.Description,
                })
                .ToListAsync();

            foreach (var element in elements)
            {
                element.Link = new Link(_linkGenerator.GetUriByAction(HttpContext, "GetElement", "Element", values: new { id = element.ElementId }), "element", "GET");
            }

            return elements;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(string id, ColorPut colorPut)
        {
            if (colorPut.Description == null)
                throw new ArgumentException();

            return await PutSingle(
                id,
                () => new Color(id, colorPut.Description),
                x =>
                {
                    x.Description = colorPut.Description;
                },
                nameof(GetColor));
        }
    }

    public class ColorView
    {
        public string? ColorId { get; set; }
        public string? Description { get; set; }
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
