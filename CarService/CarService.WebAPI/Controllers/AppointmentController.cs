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
    [Authorize(Roles ="mechanic")]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly CarServiceContext _context;

        public AppointmentController(CarServiceContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAppointments()
        {
            try
            {
                int userId = GetUserId();
                List<int> appointmensHavingWorksheet = _context.Worksheets.Select(w => w.AppointmentId).ToList();
                return Ok(_context.Appointments
                    .Where(a => a.Mechanic.Id == userId && a.Time>= DateTime.Now.Date && !appointmensHavingWorksheet.Contains(a.Id))
                    .OrderBy(a => a.Time)
                    .ToList()
                    .Select(a => new AppointmentDTO
                    {
                        Id = a.Id,
                        Time = a.Time,
                        WorkType = a.WorkType,
                        Partner = new UserDTO
                        {
                            Id = a.Partner.Id,
                            Name = a.Partner.Name
                        },
                        Mechanic = new UserDTO
                        {
                            Id = a.Mechanic.Id,
                            Name = a.Mechanic.Name
                        }
                    }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        protected virtual int GetUserId()
        {            
            return Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}