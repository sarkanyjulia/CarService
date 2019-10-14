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
        private readonly UserManager<Partner> _userManager;
        private readonly SignInManager<Partner> _signInManager;

        public AccountController(ICarServiceService service, UserManager<Partner> userManager, SignInManager<Partner> signInManager)
        {
            _service = service;
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
            return RedirectToAction("Index", "Home"); 
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

		[HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel user)
        {
            // végrehajtjuk az ellenőrzéseket
            if (!ModelState.IsValid)
                return View("Register", user);

            Partner partner = new Partner
            {
                UserName = user.UserName,               
                Name = user.Name,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };
            var result = await _userManager.CreateAsync(partner, user.Password);
            if (!result.Succeeded)
            {
                // Felvesszük a felhasználó létrehozásával kapcsolatos hibákat.
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View("Register", user);
            }

            await _signInManager.SignInAsync(partner, false); // be is jelentkeztetjük egyből a felhasználót
            return RedirectToAction("Index", "Home"); // átirányítjuk a főoldalra
        }
    }
}