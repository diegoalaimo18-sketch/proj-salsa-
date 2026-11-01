using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace Sitio.Models
{
    public class TipoMembresia
    {
        public int IdTipoMembresia { get; set; }

        public string Nombre { get; set; }

        public int DuracionDias { get; set; }

        public int CantidadClases { get; set; }

        public bool EsIlimitada { get; set; }

        public decimal Precio { get; set; }

        public TipoMembresia()
        { }

        public TipoMembresia(int idTipoMembresia, string nombre, int duracionDias,int cantidadClases,bool esIlimitada,decimal precio)
        {
            IdTipoMembresia = idTipoMembresia;
            Nombre = nombre;
            DuracionDias = duracionDias;
            CantidadClases = cantidadClases;
            EsIlimitada = esIlimitada;
            Precio = precio;
        }
      
    }
}