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
        private static UserManager<AppUser> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;

        public static void Initialize(CarServiceContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _context.Database.EnsureCreated();

            if (_context.Users.Any())
            {
                return; // Az adatbázis már inicializálva van.
            }

            SeedUsers();
            SeedMechanics();                     
        }      

        private static void SeedUsers()
        {            
            var defaultPartners = new AppUser[]
            {
                new AppUser
                {
                    UserName = "anna123",
                    Name="Anna",
                    Address = "Fő utca 1.",
                    PhoneNumber = "1234567",
                    UserType = "partner"
                },
                new AppUser
                {
                    UserName = "peti123",
                    Name="Peti",
                    Address = "Fő utca 1.",
                    PhoneNumber = "1234568",
                    UserType = "partner"
                },
                new AppUser
                {
                    UserName = "gergo123",
                    Name="Gergő",
                    Address = "Fő utca 1.",
                    PhoneNumber = "1234569",
                    UserType = "partner"
                }
            };            
            
            foreach (AppUser p in defaultPartners)
            {                
                var partnerRole = new IdentityRole<int>("partner");
                var result1 = _userManager.CreateAsync(p, "Password123").Result;
                var result2 = _roleManager.CreateAsync(partnerRole).Result;
                var result3 = _userManager.AddToRoleAsync(p, partnerRole.Name).Result;
            }            
            _context.SaveChanges();
        }
        private static void SeedMechanics()
        {
            var defaultMechanics = new AppUser[]
            {
                new AppUser
                {
                    UserName = "szerelo1",
                    Name = "Első szerelő",
                    UserType = "mechanic"
                },
                new AppUser
                {
                    UserName = "szerelo2",
                    Name = "Második szerelő",
                    UserType = "mechanic"
                },
                new AppUser
                {
                    UserName = "szerelo3",
                    Name = "Harmadik szerelő",
                    UserType = "mechanic"
                }
            };
            foreach (AppUser m in defaultMechanics)
            {
                var mechanicRole = new IdentityRole<int>("mechanic");
                var result1 = _userManager.CreateAsync(m, "Password123").Result;
                var result2 = _roleManager.CreateAsync(mechanicRole).Result;
                var result3 = _userManager.AddToRoleAsync(m, mechanicRole.Name).Result;
            }
            _context.SaveChanges();
        }
    }
}
