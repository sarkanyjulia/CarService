using System;
using System.Collections.Generic;
using System.Linq;
using CarService.Persistence;
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
                .Where(reservation => reservation.Time.Date.Equals(date))                
                .OrderBy(reservation => reservation.Time);
        }

        public Mechanic GetMechanic(int? id)
        {
            throw new NotImplementedException();
        }

        public Appointment Reservation(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
