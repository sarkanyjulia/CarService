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

        
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
       
        

        [Required]
        public int MechanicId { get; set; }

        [Required]
        public String MechanicName { get; set; }

        [Required]
        public WorkType WorkType { get; set; }

        [MaxLength(300)]
        public String Note { get; set; }

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
