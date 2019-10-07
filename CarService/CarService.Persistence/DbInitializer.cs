using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarService.Persistence
{
    public class DbInitializer
    {
        private static CarServiceContext _context;
        private static UserManager<Partner> _userManager;

        public static void Initialize(CarServiceContext context, UserManager<Partner> userManager)
        {
            _context = context;
            _userManager = userManager;
            _context.Database.EnsureCreated();

            if (_context.Mechanics.Any())
            {
                return; // Az adatbázis már inicializálva van.
            }

            SeedUsers();
            SeedMechanics();                     
        }      

        private static void SeedUsers()
        {
            Partner defaultUser = new Partner();
            defaultUser.Name="Anna";
            defaultUser.Address = "Fő utca 1.";
            defaultUser.UserName = "anna";
            var result = _userManager.CreateAsync(defaultUser, "Password123").Result;
            _context.SaveChanges();
        }
        private static void SeedMechanics()
        {
            var defaultMechanics = new Mechanic[]
            {
                new Mechanic
                {
                    Name = "Peti",
                },
                new Mechanic
                {
                    Name = "Gergő",
                }
            };
            foreach (Mechanic m in defaultMechanics)
            {
                _context.Mechanics.Add(m);
            }
            _context.SaveChanges();
        }
    }
}
