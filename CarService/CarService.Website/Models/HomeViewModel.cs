using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CarService.Website.Models
{
    public class HomeViewModel
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public List<String> Mechanics { get; set; }

        public List<List<String>> Timeslots { get; set; }

        public HomeViewModel()
        {
            Timeslots = new List<List<String>>();
            for (int i=0; i<8; ++i)
            {
                Timeslots.Add(new List<String>());
            }
        }
    }
}
