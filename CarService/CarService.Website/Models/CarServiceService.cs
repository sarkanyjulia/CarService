using System;
using System.Collections.Generic;
using System.Linq;
using CarService.Persistence;
namespace CarService.Website.Models
{
    public class CarServiceService : ICarServiceService
    {
        private readonly CarServiceContext _context;
    }
}
