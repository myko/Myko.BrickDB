using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Myko.BrickDB.Controllers
{
    [Route("api/v1/designs")]
    [ApiController]
    public class DesignController : BrickDbControllerBase<Design>
    {
        private readonly LinkGenerator _linkGenerator;

        public DesignController(BrickDbContext context, LinkGenerator linkGenerator)
            : base(context)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DesignListView>>> GetDesigns()
        {
            return await _context.Designs
                .Select(x => new DesignListView { DesignId = x.DesignId, Description = x.Description })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public Task<ActionResult<DesignView>> GetDesign(string id)
        {
            return GetSingle(
                x => x.DesignId == id,
                x => new DesignView
                {
                    DesignId = x.DesignId,
                    Description = x.Description,
                });
        }

        [HttpGet("{id}/elements")]
        public async Task<ActionResult<IEnumerable<ElementLinkView>>> GetDesignElements(string id)
        {
            if (!await _context.Designs.AnyAsync(x => x.DesignId == id))
                return NotFound();

            var elements = await _context.Elements
                .Where(x => x.Design != null && x.Design.DesignId == id)
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
        public async Task<IActionResult> PutDesign(string id, DesignPut designPut)
        {
            if (designPut.Description == null)
                throw new ArgumentException();

            return await PutSingle(
                id,
                () => new Design(id, designPut.Description),
                x =>
                {
                    x.Description = designPut.Description;
                },
                nameof(GetDesign));
        }
    }

    public class DesignView
    {
        public string DesignId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class DesignListView
    {
        public string DesignId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class DesignPut
    {
        public string? Description { get; set; }
    }
}
