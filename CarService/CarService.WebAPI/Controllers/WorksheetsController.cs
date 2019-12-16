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
    public class WorksheetsController : ControllerBase
    {
        private readonly CarServiceContext _context;

        public WorksheetsController(CarServiceContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }
        
        [HttpPost]
        public IActionResult PostWorksheet([FromBody] WorksheetDTO worksheetDTO)
        {
            int userId = GetUserId();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Worksheet worksheetToAdd = new Worksheet
                {
                    AppointmentId = worksheetDTO.Appointment.Id,
                    FinalPrice = worksheetDTO.FinalPrice,                  
                };
                worksheetToAdd.Items = new List<WorksheetWorkItem>();
                foreach (WorkItemDTO item in worksheetDTO.Items)
                {                    
                    worksheetToAdd.Items.Add(new WorksheetWorkItem
                    {
                        Item = item.Item,
                        Price = item.Price
                    });
                }
                var addedWorksheet = _context.Worksheets.Add(worksheetToAdd);
                _context.SaveChanges();
                return CreatedAtRoute("GetWorksheet", new { id = addedWorksheet.Entity.Id }, worksheetDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        protected virtual int GetUserId()
        {
            return Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}