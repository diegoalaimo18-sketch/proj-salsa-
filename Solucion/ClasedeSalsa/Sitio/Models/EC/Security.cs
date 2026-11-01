using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio.Models.EC
{
    using System;
    using System.Security.Cryptography;

    namespace Sitio.Models
    {
        public class Segurity
        {
            public static string CrearHash(string clave)
            {
                byte[] sal = new byte[16];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(sal);

                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(clave, sal, 10000);
                byte[] hash = pbkdf2.GetBytes(32);

                return Convert.ToBase64String(sal) + ":" + Convert.ToBase64String(hash);
            }

            public static bool Verificar(string clave, string guardado)
            {
                string[] partes = guardado.Split(':');
                byte[] sal = Convert.FromBase64String(partes[0]);

                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(clave, sal, 10000);
                string hashNuevo = Convert.ToBase64String(pbkdf2.GetBytes(32));

                return hashNuevo == partes[1];
            }
        }
    }
}