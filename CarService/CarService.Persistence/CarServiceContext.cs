using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarService.Persistence
{
    public class CarServiceContext : IdentityDbContext<Partner, IdentityRole<int>, int>
    {
        public CarServiceContext(DbContextOptions<CarServiceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Partner>().ToTable("Partners");
        }

        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
