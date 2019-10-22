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
    [Authorize]
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
            List<Mechanic> mechanics = _service.Mechanics.ToList();

            Dictionary<int, int> columns = new Dictionary<int, int>();
            foreach (Mechanic m in mechanics)
            {
                columns.Add(m.Id, mechanics.IndexOf(m));
            }

                model.Date = date;
            model.Mechanics = mechanics.Select(m => m.Name).ToList();
            
            for (int i=0; i<8; ++i)
            {
                foreach(Mechanic m in mechanics)
                {
                    DateTime start = baseTime.AddHours(i);
                    TimeslotViewModel item = new TimeslotViewModel(start, m.Id, m.Name);                    
                    if (start<DateTime.Now || IsHoliday(start))
                    {
                        item.Status = TimeslotStatus.DISABLED;
                    }
                    model.Timeslots[i].Add(item);
                }
            }

            foreach (Appointment r in appointments)
            {
                TimeslotStatus status;
                if (r.Partner.Equals(User.Identity))
                {
                    status = TimeslotStatus.OWN;
                }
                else
                {
                    status = TimeslotStatus.BOOKED;
                }
                model.Timeslots[r.Time.Hour - 9][columns.GetValueOrDefault(r.Mechanic.Id)].Status = status;
            }
           
            return model;
        }

        private bool IsHoliday(DateTime start)
        {
            if (start.DayOfWeek.Equals(DayOfWeek.Saturday)) return true;
            if (start.DayOfWeek.Equals(DayOfWeek.Sunday)) return true;
            return false;
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

        public IActionResult Book()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
