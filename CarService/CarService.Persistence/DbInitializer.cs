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
            var defaultPartners = new Partner[]
            {
                new Partner
                {
                    UserName = "anna123",
                    Name="Anna",
                    Address = "Fő utca 1.",
                    PhoneNumber = "1234567"
                },
                new Partner
                {
                    UserName = "peti123",
                    Name="Peti",
                    Address = "Fő utca 1.",
                    PhoneNumber = "1234568"
                },
                new Partner
                {
                    UserName = "gergo123",
                    Name="Gergő",
                    Address = "Fő utca 1.",
                    PhoneNumber = "1234569"
                }
            };            
            foreach (Partner p in defaultPartners)
            {
                var result = _userManager.CreateAsync(p, "Password123").Result;
            }            
            _context.SaveChanges();
        }
        private static void SeedMechanics()
        {
            var defaultMechanics = new Mechanic[]
            {
                new Mechanic
                {
                    Name = "Első szerelő"
                },
                new Mechanic
                {
                    Name = "Második szerelő",
                },
                new Mechanic
                {
                    Name = "Harmadik szerelő",
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
