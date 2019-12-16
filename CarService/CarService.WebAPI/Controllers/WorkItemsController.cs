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

       
    }
}