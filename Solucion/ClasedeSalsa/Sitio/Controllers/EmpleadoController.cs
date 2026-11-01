using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Sitio.Models;
using Sitio.Models.EC.Sitio.Models;

namespace Sitio.Controllers
{
    public class EmpleadoController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult Logueo()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Logueo(Empleado empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    new EmpleadoDB().Logueo(empleado);

                    empleado.PassUsu = null;
                    Session["EmpleadoLogueado"] = empleado;
                    return RedirectToAction("ListarMembresiasActivas", "Membresia");
                }

                else
                    return View();
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public ActionResult Deslogueo()
        {
            Session.Clear();
            return RedirectToAction("index", "Home");
           
        }

        
    }
}