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
                "9:00-10:00", "10:00-11:00", "11:00-12:00", "12:00-13:00", "13:00-14:00", "14:00-15:00", "15:00-16:00", "16:00-17:00"
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

        public TimeslotViewModel()
        {
            Status = TimeslotStatus.FREE;
        }

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
