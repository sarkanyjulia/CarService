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
    [Authorize(Roles = "partner")]
    public class AppointmentController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;

        public AppointmentController(ICarServiceService service, UserManager<AppUser> userManager) : base(service)
        {
            _userManager = userManager;
        }

        public IActionResult NewAppointment(DateTime? start, int? mechanicId)
        {            
            if (start == null || mechanicId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            AppUser mechanic = _service.GetMechanic(mechanicId);
            if (mechanic==null)
            {
                return RedirectToAction("Index", "Home");
            }
            else {
                AppointmentViewModel model = new AppointmentViewModel
                {
                    Start = start.Value,
                    MechanicId = mechanicId.Value,
                    MechanicName = mechanic.Name
                };
                return View("NewAppointment", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            AppointmentDateError result = _service.ValidateDate(model.Start, User.Identity.Name, model.MechanicId);
            if (result != AppointmentDateError.None)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                if (result == AppointmentDateError.InvalidDate)
                {
                    errorModel.ErrorMessage = "Sikertelen művelet - a megadott időpont nem foglalható.";
                }
                else if (result == AppointmentDateError.Conflicting)
                {
                    errorModel.ErrorMessage = "Sikertelen művelet - a megadott időpontra már létezik foglalás.";
                }
                else if (result == AppointmentDateError.ConflictingWithOwn)
                {
                    errorModel.ErrorMessage = "Sikertelen művelet - a megadott időpontra önnek már létezik foglalása.";
                }
                return View("Error", errorModel);
            }

            if (!ModelState.IsValid)
                return View("NewAppointment", model);

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

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Appointment appointment = _service.GetAppointment(id);
            if (appointment == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!appointment.Partner.UserName.Equals(User.Identity.Name))
            {
                return RedirectToAction("Index", "Home");
            }
            AppointmentViewModel model = new AppointmentViewModel
            {
                Id = appointment.Id,
                Start = appointment.Time,
                MechanicId = appointment.Mechanic.Id,
                MechanicName = appointment.Mechanic.Name,
                WorkType = (WorkType) Enum.Parse(typeof(WorkType), appointment.WorkType),
                Note = appointment.Note
            };
            return View("AppointmentDetails", model);
        }

        public IActionResult Cancel(DateTime start)
        {
            return RedirectToAction("ResetDate", "Home", start);
        }

        public IActionResult Delete(int id, DateTime start)
        {            
            Boolean success = _service.DeleteAppointment(id, User.Identity.Name);
            if (!success)
            {
                ErrorViewModel model = new ErrorViewModel();
                model.ErrorMessage = "A foglalás törlése nem sikerült.";
                return View("Error", model);
            }
            return RedirectToAction("ResetDate", "Home", start);
        }
    }
}