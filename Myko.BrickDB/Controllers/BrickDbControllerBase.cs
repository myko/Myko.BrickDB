using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Myko.BrickDB.Controllers
{
    public abstract class BrickDbControllerBase<T> : ControllerBase where T: class
    {
        protected BrickDbContext _context;

        public BrickDbControllerBase(BrickDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<T>> Delete(string id)
        {
            var set = _context.Set<T>();
            var entity = await set.FindAsync(id);
            if (entity == null)
                return NotFound();

            set.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        protected async Task<ActionResult<S>> GetSingle<S>(Expression<Func<S, bool>> condition, Expression<Func<T, S>> projection)
        {
            var set = _context.Set<T>();
            var model = await set
                .Select(projection)
                .SingleOrDefaultAsync(condition);

            if (model == null)
                return NotFound();

            return model;
        }

        protected async Task<IActionResult> PutSingle(object id, Func<T> create, Action<T> update, string actionName)
        {
            var set = _context.Set<T>();
            var entity = await set.FindAsync(id);

            if (entity == null)
            {
                entity = create();
                set.Add(entity);
            }
            else
                update(entity);

            await _context.SaveChangesAsync();

            return CreatedAtAction(actionName, new { id = id.ToString() }, entity);
        }
    }
}