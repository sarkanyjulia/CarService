using System;
using System.Collections.Generic;
using System.Linq;
using CarService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarService.Website.Models
{
    public class CarServiceService : ICarServiceService
    {
        private readonly CarServiceContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppointmentDateValidator _validator;
       

        public CarServiceService(CarServiceContext context)
        {
            _context = context;
            _validator = new AppointmentDateValidator(_context);
        }

        public IEnumerable<AppUser> Mechanics => _context.Users.Where(m => m.UserType.Equals("mechanic")).OrderBy(m =>m.Id);

        public IEnumerable<Appointment> FindAppointments(DateTime date)
        {
            return _context.Appointments
                .Include(appointment => appointment.Partner)
                .Where(appointment => appointment.Time.Date.Equals(date))                
                .OrderBy(appointment => appointment.Time);
        }

        public AppUser GetMechanic(int? id)
        {
            if (id == null)
                return null;
            AppUser mechanic = _context.Users.FirstOrDefault(m => m.Id==id);
            return mechanic;
        }

        public Appointment GetAppointment(int? id)
        {
            if (id == null)
                return null;
            Appointment appointment = _context.Appointments.Include(a => a.Mechanic).Include(a => a.Partner).FirstOrDefault(a => a.Id == id);            
            return appointment;
        }
        
        public Boolean SaveAppointment(Appointment newAppointment)
        {
            try
            {
                _context.Appointments.Add(newAppointment);                         
                _context.SaveChanges();
            } catch (Exception)
            {                
                return false;
            }
            return true;
        }

        public Boolean DeleteAppointment(int id, string userName)
        {
            try { 
                Appointment toDelete = _context.Appointments.Include(a => a.Partner).FirstOrDefault(a => a.Id == id);
                if (toDelete.Time < DateTime.Now)
                {
                    return false;
                }
                if (!userName.Equals(toDelete.Partner.UserName))
                {
                    return false;
                }
                _context.Remove(toDelete);
                _context.SaveChanges();
            } catch (Exception)
            {
                return false;
            }
            return true;
        }

        public AppointmentDateError ValidateDate(DateTime start, String username, int mechanicId)
        {
            return _validator.Validate(start, username, mechanicId);
        }
    }
}
