using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CarService.Website.Models
{
    public class HomeViewModel
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ICollection<String> Mechanics { get; set; }

        public ICollection<ICollection<String>> Timeslots { get; set; }
    }
}
