using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Sitio.Models
{


    public class Empleado
    {
        private string _usuLog;
        private string _passUsu;

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string UsuLog
        {
            get { return _usuLog; }
            set { _usuLog = value; }
        }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string PassUsu
        {
            get { return _passUsu; }
            set { _passUsu = value; }
        }

        public Empleado(string pUsuLog, string pPass)
        {
            UsuLog = pUsuLog;
            PassUsu = pPass;
        }

        public Empleado()
        {
        }
    }
}