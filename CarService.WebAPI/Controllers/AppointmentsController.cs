using System;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CarService.Data;
using CarService.Persistence;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CarService.WebAPI.Controllers
{
    [Authorize(Roles ="mechanic")]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly CarServiceContext _context;

        public AppointmentsController(CarServiceContext context)
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
                return Ok(_context.Appointments
                    .Include(a => a.Partner)
                    .Include(a => a.Worksheet)
                    .Where(a => a.Mechanic.UserName == User.Identity.Name && a.Time>= DateTime.Now.Date && a.Worksheet==null)
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
                        }
                    }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }       
    }
}