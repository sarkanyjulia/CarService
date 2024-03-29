﻿using System;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Appointment appointment = _context.Appointments.Include(a => a.Partner).Include(a => a.Mechanic).FirstOrDefault(a => a.Id==worksheetDTO.Appointment.Id);
            if (appointment==null)
            {
                return BadRequest("Appointment doesn't exist.");
            }
            if (!User.Identity.Name.Equals(appointment.Mechanic.UserName))
            {
                return Unauthorized();
            }
            try
            {
                Worksheet worksheetToAdd = new Worksheet
                {
                    AppointmentId = appointment.Id,
                    Appointment = appointment,                    
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
                return CreatedAtAction("GetWorksheet", new { id = addedWorksheet.Entity.Id }, worksheetDTO);
            }
            catch
            {                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult GetWorksheet(int id)
        {
            try
            {
                return Ok(_context.Worksheets.Find(id));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }
    }
}