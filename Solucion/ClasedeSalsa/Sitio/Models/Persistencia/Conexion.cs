using System.Configuration;

namespace Sitio.Models
{
    internal class Conexion
    {
        internal static string Cnn()
        {
            return ConfigurationManager.ConnectionStrings["ClasesDeSalsa"].ConnectionString;
        }
    }
}



