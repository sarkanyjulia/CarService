using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarService.Persistence
{
    public class Partner : IdentityUser<int>
    {
        //Id, Password, UserName, PhoneNumber

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }

        [Required]
        [MaxLength(200)]
        public String Address { get; set; }

        public ICollection<Appointment> Appointment { get; set; }

        
    }
}
