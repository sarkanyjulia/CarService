using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CarService.Website.Models
{
    public class HomeViewModel
    {
        public List<String> RowHeaders { get; }        

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public List<String> Mechanics { get; set; }

        public List<List<TimeslotViewModel>> Timeslots { get; set; }

        public HomeViewModel()
        {
            Timeslots = new List<List<TimeslotViewModel>>();
            for (int i=0; i<8; ++i)
            {
                Timeslots.Add(new List<TimeslotViewModel>());
            }
            RowHeaders = new List<string>
            {
                "9-10h", "10-11h", "11-12h", "12-13h", "13-14h", "14-15h", "15-16h", "16-17h"
            };
        }
    }

    public class TimeslotViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Start { get; }

        public TimeslotStatus Status { get; set;}
    
        public int MechanicId { get; }

        public String MechanicName { get; }

        public TimeslotViewModel(DateTime start, int mechanicId, String mechanicName)
        {
            Start = start;
            MechanicId = mechanicId;
            MechanicName = mechanicName;
            Status = TimeslotStatus.FREE;
        }

    }

    public enum TimeslotStatus
    {
        FREE, BOOKED, OWN, DISABLED
    }
}
