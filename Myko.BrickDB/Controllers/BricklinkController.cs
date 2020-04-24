using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Myko.BrickDB.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BricklinkController : ControllerBase
    {
        private readonly BrickDbContext _context;

        public BricklinkController(BrickDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostColorFile()
        {
            using (var streamReader = new StreamReader(Request.Body))
            {
                await streamReader.ReadLineAsync();
                while (true)
                {
                    var line = await streamReader.ReadLineAsync();
                    if (line == null)
                        break;

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        // 11  Black   212121  Solid   9276    10500   12491   9526    1957    2020
                        var tokens = line.Split('\t');
                        var colorId = tokens[0];
                        var description = tokens[1];

                        var color = await _context.Colors.FindAsync(colorId);
                        if (color == null)
                        {
                            color = new Color(colorId, description);
                            _context.Colors.Add(color);
                        }

                        color.Description = description;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}