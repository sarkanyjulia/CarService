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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewAppointment(DateTime start, int mechanicId, string mechanicName)
        {
            AppointmentViewModel model = new AppointmentViewModel
            {
                Start = start,
                MechanicId = mechanicId,
                MechanicName = mechanicName,
                Action = "Create",
                SubmitButtonText = "Mentés"
            };
            return View("Appointment", model);
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
                Mechanic = _service.GetMechanic(model.MechanicName),
                Partner = await _userManager.FindByNameAsync(User.Identity.Name)
            };
            _service.SaveAppointment(newAppointment);
            return RedirectToAction("ResetDate", "Home", model.Start);
        }

        public IActionResult Cancel(AppointmentViewModel model)
        {
            return RedirectToAction("ResetDate", "Home", model.Start);
        }
    }
}