﻿using CarService.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Website.Models
{
    public interface ICarServiceService
    {
        IEnumerable<Mechanic> Mechanics { get; }

        IEnumerable<Appointment> FindAppointments(DateTime date);

        Mechanic GetMechanic(int? id);

        Appointment Reservation(int? id);


    }
}
