using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarService.Persistence
{
    public class Mechanic
    {
        [Key]
        public int Id { get; set; }

        public String Name { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
