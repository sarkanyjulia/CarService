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
            HomeViewModel model = new HomeViewModel();
            model.Date = DateTime.Now.Date;
            foreach (Mechanic m in _service.Mechanics)
            {
                model.Mechanics.Add(m.Name);
            }
            
            return View("Index", model);
        }

        public IActionResult ResetDate()
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
