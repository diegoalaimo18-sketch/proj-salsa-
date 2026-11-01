using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio.Models
{
    public class OverallAverage
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public string NombreMes { get; set; }

        public decimal Total { get; set; }
        public decimal Cash { get; set; }
        public decimal Card { get; set; }

        public decimal Rent { get; set; }
        public decimal TakeHome { get; set; }
        public decimal Lee { get; set; }
        public decimal Chloe { get; set; }
        public decimal Business { get; set; }

        public string Observacion { get; set; }
    
    }

}

