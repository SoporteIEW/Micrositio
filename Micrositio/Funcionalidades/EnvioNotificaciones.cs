using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Micrositio.Funcionalidades
{
    public class EnvioNotificaciones
    {
        public dynamic EnviarNotificacion( string Numero , string ruta)
        {

            try
            {

                

               

                    var consulta = new List<dynamic>();
                    using (SqlConnection oConexion = new SqlConnection(ConfigurationManager.AppSettings.Get("RutaConexion")))
                    {
                        SqlCommand cmd = new SqlCommand("sp_ListarCorreosNotificaciones", oConexion);
                        cmd.CommandType = CommandType.StoredProcedure;

                        oConexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                consulta.Add(new
                                {
                                    CorreoElectronico = Convert.ToString(dr["CorreoElectronico"] is DBNull ? "" : dr["CorreoElectronico"]),
                                });
                            }

                        }
                    }

                    foreach (var item in consulta)
                    {
                        MailMessage correo = new MailMessage();
                        correo.From = new MailAddress("notificaciones@iewdevelopment.com.co", "Nueva Pieza", System.Text.Encoding.UTF8);
                        correo.To.Add(item.CorreoElectronico);
                        correo.Body = "Nueva pieza para el proceso " + Numero + "en la ruta "+ ruta;
                        correo.IsBodyHtml = false;
                        correo.Subject = "Nueva Pieza micrositio " + Numero;
                        correo.Priority = MailPriority.Low;
                        SmtpClient smtp = new SmtpClient();
                        smtp.UseDefaultCredentials = false;
                        smtp.Host = "smtp.dondominio.com"; //Host del servidor de correo
                        smtp.Port = 587; //Puerto de salida
                        smtp.Credentials = new System.Net.NetworkCredential("notificaciones@iewdevelopment.com.co", "Pruebas123*");//Cuenta de correo
                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        smtp.EnableSsl = true;//True si el servidor de correo permite ssl
                        smtp.Send(correo);

                    }
                
                return 200;
            }
            catch (Exception ex)
            {

                return ex;
            }


        }

    }
}
