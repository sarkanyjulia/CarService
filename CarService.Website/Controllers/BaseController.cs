using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarService.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarService.Website.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ICarServiceService _service;

        public BaseController(ICarServiceService service)
        {
            _service = service;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            ViewBag.CurrentPartnerName = String.IsNullOrEmpty(User.Identity.Name) ? null : User.Identity.Name;
        }
    }
}