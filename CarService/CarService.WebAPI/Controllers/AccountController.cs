using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarService.Persistence;

namespace CarService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet("login/{userName}/{userPassword}")]
        public async Task<IActionResult> Login(String userName, String userPassword)
        {
            try
            {                
                var result = await _signInManager.PasswordSignInAsync(userName, userPassword, false, false);
                if (!result.Succeeded)
                    return Forbid();
                             
                return Ok();
            }
            catch
            {               
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {                
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}