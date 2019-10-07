using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using CarService.Website.Models;
using Microsoft.AspNetCore.Identity;
using CarService.Persistence;
using System.Threading.Tasks;

namespace CarService.Website.Controllers
{
    public class AccountController : Controller
    {

        protected readonly ICarServiceService _service;
        protected readonly ApplicationState _applicationState;
        private readonly UserManager<Partner> _userManager;
        private readonly SignInManager<Partner> _signInManager;

        public AccountController(ICarServiceService service, ApplicationState applicationState, UserManager<Partner> userManager, SignInManager<Partner> signInManager)
        {
            _service = service;
            _applicationState = applicationState;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Login", user);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.UserPassword, user.RememberLogin, false);
            if (!result.Succeeded)
            {               
                ModelState.AddModelError("", "Hibás felhasználónév, vagy jelszó.");
                return View("Login", user);
            }
                 
            _applicationState.UserCount++; 
            return RedirectToAction("Index", "Home"); 
        }
    }
}