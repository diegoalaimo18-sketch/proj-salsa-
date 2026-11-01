using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Sitio.Models;

namespace Sitio.Controllers
{
    [SesionRequerida]
    public class MiembroController : Controller
    {
        private MiembroDB miembroDB = new MiembroDB();

        [HttpGet]
        public ActionResult RegistrarMiembro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarMiembro(Miembro miembro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    miembroDB.AgregarMiembro(miembro);

                    TempData["SuccessMessage"] = "Member successfully registered.";
                    return RedirectToAction("ListarMembresiasActivas", "Membresia");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(miembro);
        }
    }



}
