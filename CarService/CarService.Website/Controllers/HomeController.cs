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
            model.Date = date;
            model.Mechanics = _service.Mechanics.Select(m => m.Name).ToList();
            List<Reservation> reservations = _service.FindReservations(model.Date).ToList();
            for (int i=0; i<8; ++i)
            {
                foreach(String m in model.Mechanics)
                {
                    model.Timeslots[i].Add("");
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
