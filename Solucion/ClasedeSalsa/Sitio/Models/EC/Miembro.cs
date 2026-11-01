using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;


namespace Sitio.Models
{
    public class Miembro
    {
        public int IdMiembro { get; set; }
        
        [Required(ErrorMessage = "enter a name")]
        public string NombreCompleto { get; set; }
       
        [Required(ErrorMessage = "enter a telephone")]
        [RegularExpression(@"^\+?[0-9]+$", ErrorMessage = "Enter a valid telephone number")]
        [StringLength(15)]
        public string Telefono { get; set; }
       
        [Required(ErrorMessage = "enter a mail")]
        public string Mail { get; set; }


        public Membresia Membresia { get; set; }


        public Miembro(int idMiembro, string nombreCompleto, string telefono, string mail)
        {
            IdMiembro = idMiembro;
            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            Mail = mail;
       
        }

        public Miembro()
        {
    
        }


    }
}