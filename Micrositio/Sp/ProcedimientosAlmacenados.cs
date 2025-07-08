using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Micrositio.Modelos;

namespace Micrositio.Sp
{
    public class ProcedimientosAlmacenados
    {
        public List<RegistroSacosConsultar> ObternerRegistroConsultar()
        {
            var consulta = new List<RegistroSacosConsultar>();
            try
            {

             
                using (SqlConnection oConexion = new SqlConnection(ConfigurationManager.AppSettings.Get("RutaConexion")))
                {
                    SqlCommand cmd = new SqlCommand("Ups_RegistroConsultarMicrositio", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@IdUsuario", Usuario);
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            consulta.Add(new RegistroSacosConsultar
                            {                                
                                ID = Convert.ToInt32(dr["ID"] is DBNull ? "" : dr["ID"]),
                                JUZGADO = Convert.ToString(dr["JUZGADO"] is DBNull ? "" : dr["JUZGADO"]),
                                TIPO_ENTE = Convert.ToString(dr["TIPO_ENTE2"] is DBNull ? "" : dr["TIPO_ENTE2"]),
                                TIPO_ENTE2 = Convert.ToString(dr["TIPO_ENTE"] is DBNull ? "" : dr["TIPO_ENTE"]),
                                DPTO_RADICACION = Convert.ToString(dr["DPTO_RADICACION"] is DBNull ? "" : dr["DPTO_RADICACION"]),
                                CIUDAD_RADICACION = Convert.ToString(dr["CIUDAD_RADICACION"] is DBNull ? "" : dr["CIUDAD_RADICACION"]),                               
                            });
                        }

                    }
                    oConexion.Close();
                }
                return consulta;
            }
            catch (Exception ex)
            {

                return consulta;
            }
        }


        public dynamic InsertRutaPrincipal(string NumeroProceso, string FechaDocumento, string RutaDescarga, string URLCarpetaVirtual ,string RutaDocumentoMicrositio, string NumeroEncontrado, string NombreDocumento)
        {

            try
            {
                int rowsAffected = 0;
                string connectionString = ConfigurationManager.AppSettings.Get("RutaConexion");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertarProcesoMicrositio", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NumeroProceso", NumeroProceso);
                        cmd.Parameters.AddWithValue("@FechaDocumento", FechaDocumento);
                        cmd.Parameters.AddWithValue("@NombreDocumento", NombreDocumento);
                        cmd.Parameters.AddWithValue("@RutaDescarga", RutaDescarga);
                        cmd.Parameters.AddWithValue("@URLCarpetaVirtual", URLCarpetaVirtual);
                        cmd.Parameters.AddWithValue("@RutaDocumentoMicrositio", RutaDocumentoMicrositio);
                        cmd.Parameters.AddWithValue("@FechaInsercion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@NumeroEncontrado", NumeroEncontrado);
                        rowsAffected = cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    return rowsAffected;
                }


            }
            catch (Exception ex)
            {
                return ex;
            }

        }



        public dynamic ObternerListadoPorJuzgado(string TIPO_ENTE, string JUZGADO, string DPTO_RADICACION, string CIUDAD_RADICACION)
        {
            try
            {

                var consulta = new List<dynamic>();
                using (SqlConnection oConexion = new SqlConnection(ConfigurationManager.AppSettings.Get("RutaConexion")))
                {
                    SqlCommand cmd = new SqlCommand("Ups_ListadoPorJuzgado", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TIPO_ENTE", TIPO_ENTE);
                    cmd.Parameters.AddWithValue("@JUZGADO", JUZGADO);
                    cmd.Parameters.AddWithValue("@DPTO_RADICACION", DPTO_RADICACION);
                    cmd.Parameters.AddWithValue("@CIUDAD_RADICACION", CIUDAD_RADICACION);
              
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            consulta.Add(new
                            {
                                NUMERO_PROCESO = Convert.ToString(dr["NUMERO_PROCESO"] is DBNull ? "" : dr["NUMERO_PROCESO"]),
                                NOMBRE_TITULAR = Convert.ToString(dr["NOMBRE_TITULAR"] is DBNull ? "" : dr["NOMBRE_TITULAR"]),
                               
                            });
                        }

                    }
                    oConexion.Close();
                }
                return consulta;
            }
            catch (Exception ex)
            {

                return ex;
            }
        }


        public dynamic InsertarConsultasPorCarpetaMicrositio(string ID_Carpeta, string URL_Carpeta, DateTime Fecha_Consulta, string Tipo_Ente, string Ciudad, string Departamento, string Año, string Mes, bool error, string menssageError, string Juzgado , int IdJuzgado)
        {

            try
            {
                int rowsAffected = 0;
                string connectionString = ConfigurationManager.AppSettings.Get("RutaConexion");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertarConsultasPorCarpetaMicrositio", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Carpeta", ID_Carpeta);
                        cmd.Parameters.AddWithValue("@URL_Carpeta", URL_Carpeta);
                        cmd.Parameters.AddWithValue("@Fecha_Consulta", Fecha_Consulta);
                        cmd.Parameters.AddWithValue("@Tipo_Ente", Tipo_Ente);
                        cmd.Parameters.AddWithValue("@Ciudad", Ciudad);
                        cmd.Parameters.AddWithValue("@Departamento", Departamento);
                        cmd.Parameters.AddWithValue("@Año", Año);
                        cmd.Parameters.AddWithValue("@Mes", Mes);
                        cmd.Parameters.AddWithValue("@Error", error);
                        cmd.Parameters.AddWithValue("@menssageError", menssageError);
                        cmd.Parameters.AddWithValue("@Juzgado", Juzgado);
                        cmd.Parameters.AddWithValue("@IdJuzgado", IdJuzgado);
                        rowsAffected = cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    return rowsAffected;
                    
                }


            }
            catch (Exception ex)
            {
                return ex;
            }

        }



        public dynamic ConsultaPorCarpetaMicrositio(string ID_Carpeta, string URL_Carpeta)
        {
            try
            {

                var consulta = new List<dynamic>();
                using (SqlConnection oConexion = new SqlConnection(ConfigurationManager.AppSettings.Get("RutaConexion")))
                {
                    SqlCommand cmd = new SqlCommand("ConsultaPorCarpetaMicrositio", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Carpeta", ID_Carpeta);
                    cmd.Parameters.AddWithValue("@URL_Carpeta", URL_Carpeta);
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            consulta.Add(new
                            {
                                ID_Carpeta = Convert.ToString(dr["ID_Carpeta"] is DBNull ? "" : dr["ID_Carpeta"])                              

                            });
                        }

                    }
                    oConexion.Close();
                }
                return consulta;
            }
            catch (Exception ex)
            {

                return ex;
            }
        }


        public dynamic actualizarfechaconsultaJuzgado(int id)
        {

            try
            {
                int rowsAffected = 0;
                string connectionString = ConfigurationManager.AppSettings.Get("RutaConexion");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("actualizarfechaconsultaJuzgado", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);                   
                        rowsAffected = cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    return rowsAffected;
                }


            }
            catch (Exception ex)
            {
                return ex;
            }

        }


        public dynamic InsertarProcesosBusqueda(string NumeroProceso, bool Encontrado, string CiudadBusqueda, string DepartamentoBusqueda, string AnioBusqueda, string MesBusqueda ,string Ente)
        {

            try
            {
                int rowsAffected = 0;
                string connectionString = ConfigurationManager.AppSettings.Get("RutaConexion");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertarProcesoBusqueda", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NumeroProceso", NumeroProceso);
                        cmd.Parameters.AddWithValue("@Encontrado", Encontrado);
                        cmd.Parameters.AddWithValue("@CiudadBusqueda", CiudadBusqueda);
                        cmd.Parameters.AddWithValue("@DepartamentoBusqueda", DepartamentoBusqueda);
                        cmd.Parameters.AddWithValue("@AnioBusqueda", AnioBusqueda);
                        cmd.Parameters.AddWithValue("@MesBusqueda", MesBusqueda);
                        cmd.Parameters.AddWithValue("@FechaBusqueda", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Ente", Ente);
                        rowsAffected = cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    return rowsAffected;
                }


            }
            catch (Exception ex)
            {
                return ex;
            }

        }


    }
}
