using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;
using Sitio.Models;

namespace Sitio.Models
{
    public class MiembroDB
    {
        public Miembro BuscarMiembro(int idMiembro)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn());
            Miembro _unMiembro = null;

            SqlCommand _comando = new SqlCommand("BuscarMiembro", oConexion);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@IdMiembro", idMiembro);

            try
            {
                oConexion.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.Read())
                {
                

                    _unMiembro = new Miembro();
                    _unMiembro.IdMiembro = (int)_lector["IdMiembro"];
                    _unMiembro.NombreCompleto = _lector["NombreCompleto"].ToString();
                    _unMiembro.Telefono = _lector["Telefono"].ToString();
                    _unMiembro.Mail = _lector["Mail"].ToString();

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
            return _unMiembro;
        }



        public void AgregarMiembro(Miembro miembro)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn());
            SqlCommand oComando = new SqlCommand("AltaMiembro", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _nombre = new SqlParameter("@Nom", miembro.NombreCompleto);
            SqlParameter _telefono = new SqlParameter("@Telefono", miembro.Telefono);
            SqlParameter _mail = new SqlParameter("@Mail", miembro.Mail);

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_nombre);
            oComando.Parameters.Add(_telefono);
            oComando.Parameters.Add(_mail);
            oComando.Parameters.Add(_Retorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

              
            }
            catch (Exception ex)
            {
                throw new Exception("The system reports that the data operation failed", ex);
            }
            finally
            {
                oConexion.Close();
            }
            int retorno = (int)oComando.Parameters["@Retorno"].Value;

            if (retorno != 1)
            {
                throw new Exception("The system reports that the member could not be registered");
            }

        }
        public void ModificarMiembro(Miembro miembro )
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn());
            SqlCommand oComando = new SqlCommand("modificarMiembro", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            // Parámetros de entrada
            SqlParameter _IdMiembro = new SqlParameter("@IdMiembro", miembro.IdMiembro); 
            SqlParameter _Nombre = new SqlParameter("@Nombre", miembro.NombreCompleto);
            SqlParameter _Telefono = new SqlParameter("@Telefono", miembro.Telefono);
            SqlParameter _Mail = new SqlParameter("@Mail", miembro.Mail);

            // Parámetro de retorno
            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);        
            _Retorno.Direction = ParameterDirection.ReturnValue;

            // Agregar los parámetros a la colección
            oComando.Parameters.Add(_IdMiembro);
            oComando.Parameters.Add(_Nombre);
            oComando.Parameters.Add(_Telefono);
            oComando.Parameters.Add(_Mail);
            oComando.Parameters.Add(_Retorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery(); 
            
              
            }
            catch (Exception ex)
            {
                throw new Exception("The system reports that the data operation failed", ex);
            }
            finally
            {
                oConexion.Close();
            }
            int oAfectados = (int)oComando.Parameters["@Retorno"].Value;

    
            if (oAfectados == -1)
            {
                throw new Exception("Member not found");
            }
            else if (oAfectados == 1)
            {
                Console.WriteLine("modify member");
            }
        }
     




    }





}
