using System;
using System.Data.SqlClient;
using Sitio.Models.EC.Sitio.Models;

namespace Sitio.Models
{
    public class EmpleadoDB
    {


        public void Logueo(Empleado U)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn());

            SqlCommand _comando = new SqlCommand("Logueo", oConexion);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@UsuL", U.UsuLog);

            string guardado = null;

            try
            {
                oConexion.Open();
                SqlDataReader _lector = _comando.ExecuteReader();

                if (_lector.Read())
                {
                    guardado = _lector["Contraseña"].ToString();
                }

                _lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("The system reports that the data operation failed", ex);
            }
            finally
            {
                oConexion.Close();
            }

            if (guardado == null || !Segurity.Verificar(U.PassUsu, guardado))
            {
                throw new Exception("Invalid username or password");
            }
        }





    }
}