using System.Web.Mvc;
using System.Web.Routing;
using Sitio.Models;

namespace Sitio.Controllers
{
    public class SesionRequeridaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Empleado empleado = filterContext.HttpContext.Session["EmpleadoLogueado"] as Empleado;

            if (empleado == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Empleado", action = "Logueo" }));
            }
        }
    }
}