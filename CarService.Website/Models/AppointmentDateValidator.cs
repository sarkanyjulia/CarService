using CarService.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarService.Website.Models
{
    public class AppointmentDateValidator
    {
        private readonly CarServiceContext _context;

        public AppointmentDateValidator (CarServiceContext context)
        {
            _context = context;
        }

        public AppointmentDateError Validate(DateTime start, String username, int mechanicId)
        {
            if (start < DateTime.Now) return AppointmentDateError.InvalidDate;
            if (HolidayChecker.IsHoliday(start)) return AppointmentDateError.InvalidDate;

            IEnumerable<Appointment> appointments = _context.Appointments.Include(a => a.Partner).Include(a => a.Mechanic).Where(a => a.Time == start);
            Appointment own = appointments.Where(a => a.Partner.UserName.Equals(username)).FirstOrDefault();
            Appointment same = appointments.Where(a => a.Mechanic.Id==mechanicId).FirstOrDefault();

            if (same != null) return AppointmentDateError.Conflicting;
            if (own != null) return AppointmentDateError.ConflictingWithOwn;

            return AppointmentDateError.None;
        }
    }
}
