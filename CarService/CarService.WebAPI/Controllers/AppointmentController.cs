using System;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CarService.Data;
using CarService.Persistence;

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
                return Ok(_context.Appointments.Where(a => a.Mechanic.Id == userId && a.Time>= DateTime.Now.Date).OrderBy(a => a.Time)
                    .ToList()
                    .Select(a => new AppointmentDTO
                    {
                        
                        
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