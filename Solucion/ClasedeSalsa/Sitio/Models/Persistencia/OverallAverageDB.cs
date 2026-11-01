
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sitio.Models.Persistencia
{
    public class OverallAverageDB
    {
        public List<OverallAverage> ArmarReporteMensual(
            List<Membresia> membresias,List<OverallAverage> finanzas, int año)
        {
            List<OverallAverage> reporte = new List<OverallAverage>();

            string[] nombresMeses =  {"January", "February", "March", "April", "May", "June", "July", "August",  "September", "October", "November", "December"
    };

            for (int mes = 1; mes <= 12; mes++)
            {
                var membresiasDelMes = membresias.Where(m => m.FechaInicio.Year == año && m.FechaInicio.Month == mes).ToList();

                var finanzaDelMes = finanzas.FirstOrDefault(f => f.Año == año && f.Mes == mes);

                decimal total = membresiasDelMes.Sum(m => m.Costo);

                decimal cash = membresiasDelMes.Where(m => m.Pago == "Cash").Sum(m => m.Costo);

                decimal card = membresiasDelMes.Where(m => m.Pago == "Card").Sum(m => m.Costo);

                decimal rent = finanzaDelMes != null ? finanzaDelMes.Rent : 0;

                decimal takeHome = total - rent;

                OverallAverage item = new OverallAverage();

                item.Año = año;
                item.Mes = mes;
                item.NombreMes = nombresMeses[mes - 1];

                item.Total = total;
                item.Cash = cash;
                item.Card = card;

                item.Rent = rent;
                item.TakeHome = takeHome;

                item.Lee = takeHome * 0.45m;
                item.Chloe = takeHome * 0.45m;
                item.Business = takeHome * 0.10m;

                item.Observacion = finanzaDelMes != null ? finanzaDelMes.Observacion : string.Empty;

                reporte.Add(item);
            }

            return reporte;
        }






        public List<OverallAverage> GetMonthlyFinancialSummary(int año)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());

            List<OverallAverage> lista = new List<OverallAverage>();

            SqlCommand _comando =
                new SqlCommand("listarfinanzasmensuales", _cnn);

            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@año", año);

            try
            {
                _cnn.Open();

                SqlDataReader lector = _comando.ExecuteReader();

                while (lector.Read())
                {
                    OverallAverage finanza = new OverallAverage();

                    finanza.Año = (int)lector["Año"];
                    finanza.Mes = (int)lector["Mes"];
                    finanza.Rent = (decimal)lector["Rent"];
                    finanza.Observacion =lector["Observacion"] == DBNull.Value ? string.Empty : lector["Observacion"].ToString();

                    lista.Add(finanza);
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

    }
}
