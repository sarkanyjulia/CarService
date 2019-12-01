using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarService.Persistence
{
    public class AppUser : IdentityUser<int>
    {
        //Id, Password, UserName, PhoneNumber

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }
        
        [MaxLength(200)]
        public String Address { get; set; }

        public String UserType { get; set; }

        public ICollection<Appointment> PartnerAppointments { get; set; }

        public ICollection<Appointment> MechanicAppointments { get; set; }
    }
}
