using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Website.Models
{
    public class AppointmentViewModel : TimeslotViewModel
    {
        [Required]
        [MaxLength(50)]
        public WorkType WorkType { get; set; }

        [MaxLength(300)]
        public String Note { get; set; }

        public AppointmentViewModel(DateTime start, int mechanicId, String mechanicName) : base(start, mechanicId, mechanicName) { }

        public String Action { get; set; }

        public String SubmitButtonText { get; set; }
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
