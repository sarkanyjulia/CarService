using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarService.Persistence;

namespace CarService.WebAPI.Controllers
{
    [Authorize(Roles = "mechanic")]
    [Route("api/[controller]")]
    public class WorksheetController : ControllerBase
    {
       
    }
}