﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [MaxLength(300)]
        public String Note { get; set; }    

        [Required]
        public AppUser Partner { get; set; }

        [Required]
        public AppUser Mechanic { get; set; }
        

        public  Worksheet Worksheet { get; set; }
         
    }
}
