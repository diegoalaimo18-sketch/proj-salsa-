using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;


namespace Sitio.Models
{
    public class Membresia
    {
        public int IdMembresia { get; set; }
        public int IdMiembro { get; set; }

        //[Required(ErrorMessage = "The type of membership is required")]
        public int IdTipoMembresia { get; set; }
        public int CantClases { get; set; }
        public bool Activo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaExpiracion { get; set; }

        [Required(ErrorMessage = "The payment method is required")]
        public string Pago { get; set; }


        public decimal Costo { get; set; }
        public string Observacion { get; set; }

        public virtual Miembro Miembro { get; set; }
        public virtual TipoMembresia TipoMembresia { get; set; }

        // Constructor
        public Membresia(int idMembresia, int idMiembro, TipoMembresia tipo,int cantClases, bool activo, DateTime fechaInicio, DateTime fechaExpiracion, decimal costo, string observacion)
        {
            IdMembresia = idMembresia;
            IdMiembro = idMiembro;
            TipoMembresia = tipo;
            Activo = activo;
            FechaInicio = fechaInicio;
            FechaExpiracion = fechaExpiracion;
            Costo = costo;
            Observacion = observacion;
            CantClases = cantClases;
        }

        public Membresia() { }

     

        



        }
}
