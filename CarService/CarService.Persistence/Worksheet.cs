using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarService.Persistence
{
    public class Worksheet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        public virtual Appointment Appointment { get; set; }

        public List<WorksheetWorkItem> Items { get; set; }        

        public int FinalPrice { get; set; }

    }
}
