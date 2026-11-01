using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Sitio.Models;

namespace Sitio.Controllers
{
    [SesionRequerida]
    public class MembresiaController : Controller
    {
        private MembresiaDB membresiaDB = new MembresiaDB();//cambiar el nombre por que confunde 
        private MiembroDB miembrod = new MiembroDB();


        [HttpGet]
        public ActionResult FormAltaMembresia(int idMiembro)
        {
            Empleado empleado = Session["EmpleadoLogueado"] as Empleado;
            if (empleado == null)
                return RedirectToAction("Logueo", "Empleado");

            var miembro = miembrod.BuscarMiembro(idMiembro);
            if (miembro == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction("ListarMiembros");
            }

            var membresia = new Membresia
            {
                IdMiembro = miembro.IdMiembro,
                Miembro = miembro
            };
            return View(membresia);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormAltaMembresia(Membresia membresia)
        {
            Empleado empleado = Session["EmpleadoLogueado"] as Empleado;

            if (empleado == null)
                return RedirectToAction("Logueo", "Empleado");

            if (ModelState.IsValid)
            {
                try
                {

                    membresiaDB.AgregarMembresia(membresia);
                    TempData["SuccessMessage"] = "Membership registered.";
                    return RedirectToAction("ListarMembresiasActivas");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    membresia.Miembro = miembrod.BuscarMiembro(membresia.IdMiembro);
                    return View(membresia);
                }
            }


            return View(membresia);
        }
        public ActionResult ListarMembresiasActivas()
        {
            try
            {
                List<Membresia> membresiasActivas = membresiaDB.ListarMiembrosActivos();

                return View(membresiasActivas);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "The system reports that memberships could not be loaded";
                return View(new List<Membresia>());
            }
        }

        public ActionResult Historial()
        {
            try
            {
                List<Membresia> historial = membresiaDB.Historial();

                return View(historial);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "The system reports that memberships could not be loaded";
                return View(new List<Membresia>());
            }
        }

        public ActionResult ListarMiembros()
        {
            try
            {
                List<Membresia> miembros = membresiaDB.ListarMiembros();

                return View(miembros);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "The system reports that members could not be loaded";
                return View(new List<Membresia>());
            }
        }
        public ActionResult FiltrarListaCompleta(string searchName)
        {
            try
            {
                List<Membresia> membresias = membresiaDB.ListarMiembros();

                if (!string.IsNullOrEmpty(searchName))
                {
                    membresias = membresias.Where(m => m.Miembro.NombreCompleto.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                return View("ListarMiembros", membresias);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "The system reports that members could not be loaded";
                return View("ListarMiembros", new List<Membresia>());
            }
        }

        public ActionResult FiltrarHistorial(string searchName)
        {
            try
            {
                List<Membresia> membresias = membresiaDB.Historial();

                if (!string.IsNullOrEmpty(searchName))
                {
                    membresias = membresias.Where(m => m.Miembro.NombreCompleto.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                return View("Historial", membresias);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "The system reports that memberships could not be loaded";
                return View("Historial", new List<Membresia>());
            }
        }

        public ActionResult Filtrar(string searchName)
        {
            try
            {
                List<Membresia> membresias = membresiaDB.ListarMiembrosActivos();

                if (!string.IsNullOrEmpty(searchName))
                {
                    membresias = membresias.Where(m => m.Miembro.NombreCompleto.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                return View("ListarMembresiasActivas", membresias);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "The system reports that memberships could not be loaded";
                return View("ListarMembresiasActivas", new List<Membresia>());
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarEstadoMembresia(List<int> selectedMembers)
        {
            if (selectedMembers == null || !selectedMembers.Any())
            {
                TempData["ErrorMessage"] = "No members were selected.";
                return RedirectToAction("ListarMembresiasActivas");
            }

            try
            {
                var membresias = membresiaDB.ListarMiembrosActivos();

                var seleccionadas = membresias
                    .Where(m => selectedMembers.Contains(m.IdMembresia))
                    .ToList();

                if (!seleccionadas.Any())
                {
                    TempData["ErrorMessage"] = "No active memberships were found.";
                    return RedirectToAction("ListarMembresiasActivas");
                }

                foreach (var membresia in seleccionadas)
                {
                    membresiaDB.ActualizarMembresia(membresia);
                }

                TempData["SuccessMessage"] = "Attendance confirmed successfully.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Attendance could not be confirmed.";
            }

            return RedirectToAction("ListarMembresiasActivas");
        }

        [HttpGet]
        public ActionResult FormModificar(int idMiembro)
        {
            try
            {
                var miembro = miembrod.BuscarMiembro(idMiembro);

                if (miembro == null)
                {
                    ViewBag.ErrorMessage = "Member not found.";
                    return View();
                }

                return View(miembro);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Miembro());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormModificar(Miembro miembro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    miembrod.ModificarMiembro(miembro);
                    return RedirectToAction("ListarMiembros");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(miembro);
        }

        [HttpGet]
        public ActionResult FormMembresiaEliminar(int IdMembresia)
        {
            try
            {
                Membresia membresia = membresiaDB.BuscarMembresia(IdMembresia);

                if (membresia == null)
                    throw new Exception("Membership doesn't exist");

                return View(membresia);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Membresia());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormMembresiaEliminar(Membresia M)
        {
            try
            {
                membresiaDB.EliminarMembresia(M);
                return RedirectToAction("Historial");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Historial");
            }
        }

        [HttpGet]
        public ActionResult FormModificarObservacion(int IdMembresia)
        {
            try
            {
                var membresia = membresiaDB.BuscarMembresia(IdMembresia);

                if (membresia == null)
                {
                    ViewBag.ErrorMessage = "Membership not found";
                    return View();
                }

                return View(membresia);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Membresia());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormModificarObservacion(Membresia membresia)
        {
            try
            {
                if (membresia.IdMembresia <= 0)
                {
                    ViewBag.ErrorMessage = "Invalid MembershipId";
                    return View(membresia);
                }

                if (ModelState.IsValid)
                {
                    membresiaDB.ModificarObservacion(membresia);
                    return RedirectToAction("Historial");
                }

                return View(membresia);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(membresia);
            }
        }

    }
}
