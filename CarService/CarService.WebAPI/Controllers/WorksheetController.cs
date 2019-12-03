using System;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CarService.Data;
using CarService.Persistence;
using System.Collections.Generic;

namespace CarService.WebAPI.Controllers
{
    [Authorize(Roles = "mechanic")]
    [Route("api/[controller]")]
    public class WorksheetController : ControllerBase
    {
        private readonly CarServiceContext _context;

        public WorksheetController(CarServiceContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        /*
        [HttpPost]
        public IActionResult PostWorksheet([FromBody] worksheetDTO)
        {
            int userId = GetUserId();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
        }
        */
        protected virtual int GetUserId()
        {
            return Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}