using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarService.Persistence;

namespace CarService.WebAPI.Controllers
{
    [Authorize(Roles ="mechanic")]
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}