using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Website.Models
{
    public class AppointmentViewModel : TimeslotViewModel
    {
        public String WorkType { get; set; }

        public String Note { get; set; }

        public AppointmentViewModel(DateTime start, int mechanicId, String mechanicName) : base(start, mechanicId, mechanicName) { }
       
    }
}
