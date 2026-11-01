using Sitio.Models;

using Sitio.Models.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio.Controllers
{
    [SesionRequerida]
    public class OverallAverageController : Controller
    {
        private OverallAverageDB persistencia = new OverallAverageDB();
        private MembresiaDB membresiaDB = new MembresiaDB();

        public ActionResult SumaMensual(int? año)
        {
            int añoSeleccionado = año ?? DateTime.Now.Year;

            List<Membresia> membresias = membresiaDB.Historial();
            List<OverallAverage> finanzas = persistencia.GetMonthlyFinancialSummary(añoSeleccionado);
            List<OverallAverage> resumen = persistencia.ArmarReporteMensual(membresias, finanzas, añoSeleccionado);

            ViewBag.AñoSeleccionado = añoSeleccionado;

            return View(resumen);
        }
    }
}


