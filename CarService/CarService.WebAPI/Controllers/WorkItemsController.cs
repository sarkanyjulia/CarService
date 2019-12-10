using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarService.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace CarService.WebAPI.Controllers
{
    [Authorize(Roles = "mechanic")]
    [Produces("application/json")]
    [Route("api/WorkItems")]
    public class WorkItemsController : Controller
    {
        private readonly CarServiceContext _context;

        public WorkItemsController(CarServiceContext context)
        {
            _context = context;
        }

        // GET: api/WorkItems
        [HttpGet]
        public IEnumerable<WorkItem> GetWorkItems()
        {
            return _context.WorkItems;
        }

        // GET: api/WorkItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workItem = await _context.WorkItems.SingleOrDefaultAsync(m => m.Id == id);

            if (workItem == null)
            {
                return NotFound();
            }

            return Ok(workItem);
        }

        // PUT: api/WorkItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkItem([FromRoute] int id, [FromBody] WorkItem workItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(workItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkItemExists(id))
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

        // POST: api/WorkItems
        [HttpPost]
        public async Task<IActionResult> PostWorkItem([FromBody] WorkItem workItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WorkItems.Add(workItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkItem", new { id = workItem.Id }, workItem);
        }

        // DELETE: api/WorkItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workItem = await _context.WorkItems.SingleOrDefaultAsync(m => m.Id == id);
            if (workItem == null)
            {
                return NotFound();
            }

            _context.WorkItems.Remove(workItem);
            await _context.SaveChangesAsync();

            return Ok(workItem);
        }

        private bool WorkItemExists(int id)
        {
            return _context.WorkItems.Any(e => e.Id == id);
        }
    }
}