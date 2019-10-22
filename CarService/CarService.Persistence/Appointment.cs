using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarService.Persistence
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        [MaxLength(50)]
        public String WorkType { get; set; }

        public String Note { get; set; }

        [Required]
        public Partner Partner { get; set; }

        [Required]
        public Mechanic Mechanic { get; set; }
    }
}
