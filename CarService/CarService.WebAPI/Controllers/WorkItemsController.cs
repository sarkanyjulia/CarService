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
        public IActionResult GetWorkItems()
        {
            try
            {
                return Ok(_context.WorkItems);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
       
    }
}