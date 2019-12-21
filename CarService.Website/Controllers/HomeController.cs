using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarService.Website.Models;
using Microsoft.AspNetCore.Authorization;
using CarService.Persistence;

namespace CarService.Website.Controllers
{
    [Authorize(Roles = "partner")]
    public class HomeController : BaseController
    {

        public HomeController(ICarServiceService service) : base(service)
        {

        }

        public IActionResult Index()
        {
            HomeViewModel model = BuildHomeViewModel(DateTime.Now.Date);
            return View("Index", model);
        }

        private HomeViewModel BuildHomeViewModel(DateTime date)
        {
            HomeViewModel model = new HomeViewModel();
            DateTime baseTime = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);           
            List<Appointment> appointments = _service.FindAppointments(date).ToList();
            List<AppUser> mechanics = _service.Mechanics.ToList();

            Dictionary<int, int> columns = new Dictionary<int, int>();
            foreach (AppUser m in mechanics)
            {
                columns.Add(m.Id, mechanics.IndexOf(m));
            }

                model.Date = date;
            model.Mechanics = mechanics.Select(m => m.Name).ToList();
            
            for (int i=0; i<8; ++i)
            {
                foreach(AppUser m in mechanics)
                {
                    DateTime start = baseTime.AddHours(i);
                    TimeslotViewModel item = new TimeslotViewModel(start, m.Id, m.Name);                                        
                    model.Timeslots[i].Add(item);
                }
            }

            foreach (Appointment appointment in appointments)
            {
                TimeslotStatus status;
                if (appointment.Partner.UserName.Equals(User.Identity.Name))
                {
                    status = TimeslotStatus.OWN;
                    model.Timeslots[appointment.Time.Hour - 9][columns.GetValueOrDefault(appointment.Mechanic.Id)].Id = appointment.Id;
                }
                else
                {
                    status = TimeslotStatus.BOOKED;
                }
                model.Timeslots[appointment.Time.Hour - 9][columns.GetValueOrDefault(appointment.Mechanic.Id)].Status = status;
            }

            for (int i = 0; i < 8; ++i)
            {
                foreach (AppUser m in mechanics)
                {
                    TimeslotViewModel timeslot = model.Timeslots[i][columns.GetValueOrDefault(m.Id)];
                    if (timeslot.Start < DateTime.Now || HolidayChecker.IsHoliday(timeslot.Start))
                    {
                        timeslot.Status = TimeslotStatus.DISABLED;
                    }
                  
                }
            }

            return model;
        }

        public IActionResult ResetDate(DateTime? date)
        {
            if (date.HasValue)
            {
                HomeViewModel model = BuildHomeViewModel(date.Value);
                return View("Index", model);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
