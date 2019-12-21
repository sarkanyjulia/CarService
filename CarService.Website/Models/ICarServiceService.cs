using CarService.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Website.Models
{
    public interface ICarServiceService
    {
        IEnumerable<AppUser> Mechanics { get; }

        IEnumerable<Appointment> FindAppointments(DateTime date);

        AppUser GetMechanic(int? id);        

        Appointment GetAppointment(int? id);

        Boolean SaveAppointment(Appointment newAppointment);

        Boolean DeleteAppointment(int id, string userName);

        AppointmentDateError ValidateDate(DateTime start, String username, int mechanicId);
    }
}
