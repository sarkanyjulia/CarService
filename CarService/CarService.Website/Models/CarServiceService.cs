using System;
using System.Collections.Generic;
using System.Linq;
using CarService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarService.Website.Models
{
    public class CarServiceService : ICarServiceService
    {
        private readonly CarServiceContext _context;

        public CarServiceService(CarServiceContext context)
        {
            _context = context;
        }      

        public IEnumerable<Mechanic> Mechanics => _context.Mechanics.OrderBy(m =>m.Id);

        public IEnumerable<Appointment> FindAppointments(DateTime date)
        {
            return _context.Appointments
                .Include(appointment => appointment.Partner)
                .Where(appointment => appointment.Time.Date.Equals(date))                
                .OrderBy(appointment => appointment.Time);
        }

        public Mechanic GetMechanic(int? id)
        {
            if (id == null)
                return null;
            Mechanic mechanic = _context.Mechanics.FirstOrDefault(m => m.Id==id);
            return mechanic;
        }


        public Mechanic GetMechanic(String mechanicName)
        {
            if (mechanicName.Equals(""))
                return null;
            Mechanic mechanic = _context.Mechanics.FirstOrDefault(m => m.Name == mechanicName);
            return mechanic;
        }


        public Appointment GetAppointment(int? id)
        {
            throw new NotImplementedException();
        }

        public Boolean SaveAppointment(Appointment newAppointment)
        {
            _context.Appointments.Add(newAppointment);
            try {               
                _context.SaveChanges();
            } catch (Exception)
            {                
                return false;
            }
            return true;
        }

        public Boolean DeleteAppointment(int id)
        {
            try { 
                Appointment toDelete = _context.Appointments.Find(id);
                _context.Remove(toDelete);
                _context.SaveChanges();
            } catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
