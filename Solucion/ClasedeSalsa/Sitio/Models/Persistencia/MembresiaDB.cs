using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;


namespace Sitio.Models
{
    public class MembresiaDB
    {
        public void AgregarMembresia(Membresia membresia)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn());

            SqlCommand _comando = new SqlCommand("AltaMembresia", oConexion)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            _comando.Parameters.AddWithValue("@IdMiembro", membresia.IdMiembro);
            _comando.Parameters.AddWithValue("@IdTipoMembresia", membresia.IdTipoMembresia);
            _comando.Parameters.AddWithValue("@Observacion", (object)membresia.Observacion ?? DBNull.Value);
            _comando.Parameters.AddWithValue("@Pago", membresia.Pago);

            SqlParameter returnParameter = new SqlParameter();
            returnParameter.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(returnParameter);

            try
            {
                oConexion.Open();
                _comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("The system reports that the data operation failed", ex);
            }
            finally
            {
                oConexion.Close();
            }

            int resultado = (int)returnParameter.Value;

            if (resultado == -1)
                throw new Exception("Member not found");
            if (resultado == -2)
                throw new Exception("Membership type not found");
            if (resultado == -3)
                throw new Exception("This member already has an active membership");
            if (resultado == -4)
                throw new Exception("Invalid payment method");
        }


        public Membresia BuscarMembresia(int idMembresia)
        {
            Membresia unaMembresia = null;

            SqlConnection oConexion =
                new SqlConnection(Conexion.Cnn());

            SqlCommand oComando =
                new SqlCommand("BuscarMembresia", oConexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

            oComando.Parameters.Add("@IdMembresia", SqlDbType.Int).Value =
                idMembresia;

            try
            {
                oConexion.Open();

                SqlDataReader reader = oComando.ExecuteReader();

                if (reader.Read())
                {
                    unaMembresia = new Membresia
                    {
                        IdMembresia =Convert.ToInt32(reader["IdMembresia"]),
                        IdMiembro = Convert.ToInt32(reader["IdMiembro"]),
                        IdTipoMembresia = Convert.ToInt32(reader["IdTipoMembresia"]),
                        Activo = Convert.ToBoolean(reader["Actbt"]),
                        CantClases =Convert.ToInt32(reader["CantClases"]),
                        FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                        FechaExpiracion = Convert.ToDateTime(reader["FechaExpiracion"]),
                        Costo = Convert.ToDecimal(reader["Costo"]),
                        Observacion = reader["Observacion"] == DBNull.Value ? null : reader["Observacion"].ToString(),
                        Pago = reader["Pago"].ToString(),

                        TipoMembresia = new TipoMembresia
                        {
                            IdTipoMembresia =Convert.ToInt32(reader["IdTipoMembresia"]),
                            Nombre = reader["Nombre"].ToString()
                        }
                    };

                    MiembroDB miembroDB = new MiembroDB();
                    unaMembresia.Miembro =miembroDB.BuscarMiembro(unaMembresia.IdMiembro );
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "The system reports that the data operation failed",ex);
            }
            finally
            {
                oConexion.Close();
            }

            return unaMembresia;
        }

        public List<Membresia> ListarMiembros()
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());

            List<Membresia> lista = new List<Membresia>();

            SqlCommand _comando = new SqlCommand("ListarMiembros", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader lector = _comando.ExecuteReader();

                if (lector.HasRows)
                {
                    while (lector.Read())
                    {
                        Membresia membresia = new Membresia();

                        // Miembro
                        membresia.Miembro = new Miembro();
                        membresia.Miembro.IdMiembro =(int)lector["IdMiembro"];
                        membresia.Miembro.NombreCompleto =lector["NombreCompleto"].ToString();
                        membresia.Miembro.Telefono = lector["Telefono"].ToString();
                        membresia.Miembro.Mail = lector["Mail"].ToString();


                        // Membresia activa
                        if (lector["IdMembresia"] != DBNull.Value)
                        {
                            membresia.IdMembresia =(int)lector["IdMembresia"];
                            membresia.Activo = (bool)lector["Actbt"];
                            membresia.FechaInicio = (DateTime)lector["FechaInicio"];
                            membresia.FechaExpiracion =(DateTime)lector["FechaExpiracion"];

                            // TipoMembresia
                            membresia.TipoMembresia = new TipoMembresia();
                            membresia.TipoMembresia.IdTipoMembresia =(int)lector["IdTipoMembresia"];
                            membresia.TipoMembresia.Nombre =lector["Nombre"].ToString();
                        }

                        lista.Add(membresia);
                    }
                }

                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }

            return lista;
        }
        public List<Membresia> ListarMiembrosActivos()
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());
           
            List<Membresia> lista = new List<Membresia>();

            SqlCommand _comando = new SqlCommand("ListarMiembrosActivos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader lector = _comando.ExecuteReader();

                if (lector.HasRows)
                {
                    while (lector.Read())
                    {
                        Membresia membresia = new Membresia();

                        membresia.IdMembresia = (int)lector["IdMembresia"];
                        membresia.FechaInicio = (DateTime)lector["FechaInicio"];
                        membresia.FechaExpiracion = (DateTime)lector["FechaExpiracion"];
                        membresia.Costo = (decimal)lector["Costo"];
                        membresia.Observacion = lector["Observacion"].ToString();
                        membresia.CantClases = (int)lector["CantClases"];
                        membresia.Pago = lector["Pago"].ToString();

                        membresia.Miembro = new Miembro();
                        membresia.Miembro.IdMiembro = (int)lector["IdMiembro"];
                        membresia.Miembro.NombreCompleto = lector["NombreCompleto"].ToString();
                        membresia.Miembro.Telefono = lector["Telefono"].ToString();
                        membresia.Miembro.Mail = lector["Mail"].ToString();

                        membresia.TipoMembresia = new TipoMembresia();
                        membresia.TipoMembresia.IdTipoMembresia = (int)lector["IdTipoMembresia"];
                        membresia.TipoMembresia.Nombre = lector["Nombre"].ToString();

                        lista.Add(membresia);




                       
                    }
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }

            return lista;
        }




        public void ActualizarMembresia(Membresia membresia)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn());
            {
                using (SqlCommand oComando = new SqlCommand("actualizarMembresia", oConexion))
                {
                    oComando.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    oComando.Parameters.Add(new SqlParameter("@IdMembresia", membresia.IdMembresia));
                    
                   

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
                }
            }
        }

        public List<Membresia> Historial()
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());

            List<Membresia> lista = new List<Membresia>();

            SqlCommand _comando = new SqlCommand("Historial", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader lector = _comando.ExecuteReader();

                if (lector.HasRows)
                {
                    while (lector.Read())
                    {
                        Membresia membresia = new Membresia();

                        membresia.IdMembresia = (int)lector["IdMembresia"];
                        membresia.FechaInicio = (DateTime)lector["FechaInicio"];
                        membresia.FechaExpiracion = (DateTime)lector["FechaExpiracion"];
                        membresia.Costo = (decimal)lector["Costo"];
                        membresia.Observacion = lector["Observacion"].ToString();
                        membresia.CantClases = (int)lector["CantClases"];
                        membresia.Activo = (bool)lector["Actbt"];
                        membresia.Pago = lector["Pago"].ToString();

                        membresia.Miembro = new Miembro();
                        membresia.Miembro.IdMiembro = (int)lector["IdMiembro"];
                        membresia.Miembro.NombreCompleto = lector["NombreCompleto"].ToString();
                      

                        membresia.TipoMembresia = new TipoMembresia();
                        membresia.TipoMembresia.Nombre = lector["Nombre"].ToString();
                        lista.Add(membresia);
                    }



                }

                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                _cnn.Close();
            }

          return lista;
        }

       

        public void EliminarMembresia(Membresia unaMembresia)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn());

            SqlCommand _comando = new SqlCommand("EliminarMembresia", oConexion);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@IdMembresia",unaMembresia.IdMembresia );
            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                oConexion.Open();
                _comando.ExecuteNonQuery();
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oConexion.Close();
            }
            if ((int)_retorno.Value == -1)
{
    throw new Exception("Membership doesn't exist");
}
        }
        public void ModificarObservacion(Membresia membresia)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn());
            SqlCommand oComando = new SqlCommand("ModificarNotas", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _IdMembresia = new SqlParameter("@IdMembresia", membresia.IdMembresia);
            SqlParameter _Observacion = new SqlParameter("@Observacion", (object)membresia.Observacion ?? DBNull.Value);

            // Parámetro de retorno
            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_IdMembresia);
            oComando.Parameters.Add(_Observacion);
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

            if ((int)_Retorno.Value == -1)
            {
                throw new Exception("Membership doesn't exist");
            }
        }

    }
}



    
    
