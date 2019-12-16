using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarService.Persistence
{
    public class CarServiceContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public CarServiceContext(DbContextOptions<CarServiceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<Appointment>(entity =>
            {
                entity.HasOne(e => e.Partner)
                    .WithMany(u => u.PartnerAppointments)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.Mechanic)
                   .WithMany(u => u.MechanicAppointments)
                   .OnDelete(DeleteBehavior.ClientSetNull);   
                

            });
            
        }
        
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Worksheet> Worksheets { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorksheetWorkItem> WorksheetWorkItems { get; set; }
    }
}
