using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarService.Persistence;
using CarService.Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarService.Website.Controllers
{
    [Authorize]
    public class AppointmentController : BaseController
    {
        private readonly UserManager<Partner> _userManager;

        public AppointmentController(ICarServiceService service, UserManager<Partner> userManager) : base(service)
        {
            _userManager = userManager;
        }

        public IActionResult NewAppointment(DateTime? start, int? mechanicId, string mechanicName)
        {
            if (start == null || mechanicId == null || String.IsNullOrEmpty(mechanicName))
            {
                return RedirectToAction("Index", "Home");
            }
            else {
                AppointmentViewModel model = new AppointmentViewModel
                {
                    Start = start.Value,
                    MechanicId = mechanicId.Value,
                    MechanicName = mechanicName
                };
                return View("NewAppointment", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {



            Appointment newAppointment = new Appointment
            {
                Time = model.Start,
                WorkType = model.WorkType.ToString(),
                Note = model.Note,
                Mechanic = _service.GetMechanic(model.MechanicId),
                Partner = await _userManager.FindByNameAsync(User.Identity.Name)
            };
            _service.SaveAppointment(newAppointment);
            return RedirectToAction("ResetDate", "Home", model.Start);
        }

        public IActionResult Details(int id, DateTime start, int mechanicId, string mechanicName)
        {
            AppointmentViewModel model = new AppointmentViewModel
            {
                Id = id,
                Start = start,
                MechanicId = mechanicId,
                MechanicName = mechanicName,
            };
            return View("AppointmentDetails", model);
        }

        public IActionResult Cancel(DateTime start)
        {
            return RedirectToAction("ResetDate", "Home", start);
        }

        public IActionResult Delete(int id, DateTime start)
        {
            _service.DeleteAppointment(id);
            return RedirectToAction("ResetDate", "Home", start);
        }
    }
}