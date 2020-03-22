using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFood.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Bitte nicht leer lassen"), EmailAddress(ErrorMessage ="Dies ist keine gültige E-Mail"), DataType(DataType.EmailAddress)]
        public string Mail { get; set; }
        [Required(ErrorMessage ="Bitte nicht leer lassen"), MinLength(6, ErrorMessage ="Das Passwort muss mindestens 6 Stellen besitzen"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bitte nicht leer lassen"), Compare(nameof(Password), ErrorMessage ="Die Passwörter sind nicht gleich"), DataType(DataType.Password)]
        public string PasswordRepeat { get; set; }
    }
}
