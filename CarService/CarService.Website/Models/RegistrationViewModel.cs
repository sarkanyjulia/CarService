using System;
using System.ComponentModel.DataAnnotations;

namespace CarService.Website.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "A felhasználónév megadása kötelező.")]
        [RegularExpression("^[A-Za-z0-9_-]{5,40}$", ErrorMessage = "A felhasználónév formátuma, vagy hossza nem megfelelő.")]
        public String UserName { get; set; }
        
        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        
        [Required(ErrorMessage = "A jelszó ismételt megadása kötelező.")]
        [Compare(nameof(Password), ErrorMessage = "A két jelszó nem egyezik.")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "A név megadása kötelező.")] // feltételek a validáláshoz
        [StringLength(60, ErrorMessage = "A név maximum 60 karakter lehet.")]
        public String Name { get; set; }
        
        [Required(ErrorMessage = "A cím megadása kötelező.")]
        public String Address { get; set; }

        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [Phone(ErrorMessage = "A telefonszám formátuma nem megfelelő.")]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }
    }
}
