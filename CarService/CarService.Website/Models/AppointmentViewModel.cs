using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Website.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        [HiddenInput]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
              
        [HiddenInput]
        [Required]
        public int MechanicId { get; set; }

        [HiddenInput]
        [Required]
        public String MechanicName { get; set; }

        [Required]
        public WorkType WorkType { get; set; }

        [StringLength(300, ErrorMessage = "A megjegyzés maximum 300 karakter lehet.")]
        public String Note { get; set; }
    }

    public enum WorkType
    {
        [Display(Name ="Kötelező szervíz")]
        Maintenance,
        [Display(Name = "Műszaki vizsga")]
        Inspection,
        [Display(Name = "Meghibásodás")]
        Failure
    }
}
