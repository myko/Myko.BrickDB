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
    [Route("api/v1/elements")]
    [ApiController]
    public class ElementController : BrickDbControllerBase<Element>
    {
        private readonly LinkGenerator _linkGenerator;

        public ElementController(BrickDbContext context, LinkGenerator linkGenerator)
            : base(context)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElementListView>>> GetElements()
        {
            return await _context.Elements
                .Select(x => new ElementListView
                {
                    ElementId = x.ElementId,
                    Description = x.Description,
                    Design = x.Design!.Description,
                    Color = x.Color!.Description
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public Task<ActionResult<ElementView>> GetElement(string id)
        {
            return GetSingle(
                x => x.ElementId == id,
                x => new ElementView
                {
                    ElementId = x.ElementId,
                    Description = x.Description,
                    DesignId = x.Design!.DesignId,
                    Design = x.Design!.Description,
                    ColorId = x.Color!.ColorId,
                    Color = x.Color.Description
                });
        }

        //[HttpGet("{id}/elements")]
        //public async Task<ActionResult<IEnumerable<ElementLinkView>>> GetColorElements(string id)
        //{
        //    if (!await _context.Colors.AnyAsync(x => x.ColorId == id))
        //        return NotFound();

        //    var elements = await _context.Elements
        //        .Where(x => x.Color != null && x.Color.ColorId == id)
        //        .Select(x => new ElementLinkView
        //        {
        //            ElementId = x.ElementId,
        //            Description = x.Description,
        //        })
        //        .ToListAsync();

        //    foreach (var element in elements)
        //    {
        //        element.Link = new Link(_linkGenerator.GetUriByAction(HttpContext, "GetDesign", "Design", values: new { id = element.ElementId }), "element", "GET");
        //    }

        //    return elements;
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> PutElement(string id, ElementPut elementPut)
        {
            var element = await _context.Elements.FindAsync(id);

            if (element == null)
            {
                if (elementPut.Description == null)
                    throw new ArgumentException();

                element = new Element(id, elementPut.Description);
                _context.Elements.Add(element);
            }

            if (elementPut.Description != null)
                element.Description = elementPut.Description;
            if (elementPut.DesignId != null)
                element.Design = _context.Designs.Find(elementPut.DesignId);
            if (elementPut.ColorId != null)
                element.Color = _context.Colors.Find(elementPut.ColorId);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetElement), new { id = element.ElementId }, element);
        }
    }


    public class ElementView
    {
        public string? ElementId { get; set; }
        public string? Description { get; set; }
        public string? DesignId { get; set; }
        public string? Design { get; set; }
        public string? ColorId { get; set; }
        public string? Color { get; set; }
    }

    public class ElementLinkView
    {
        public string? ElementId { get; set; }
        public string? Description { get; set; }
        public Link? Link { get; set; }
    }

    public class ElementListView
    {
        public string? ElementId { get; set; }
        public string? Description { get; set; }
        public string? Design { get; set; }
        public string? Color { get; set; }
    }

    public class ElementPut
    {
        [Required] public string? Description { get; set; }
        public string? DesignId { get; set; }
        public string? ColorId { get; set; }
    }
}
