using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micrositio.Funcionalidades
{
    public class GuardarPieza
    {
        public string GuardarPiezas(string inputFile, int startPage, string obligacion)
        {
       

            try
            {
                bool enviarnotificacion = false;
                string connectionString = ConfigurationManager.AppSettings.Get("RutaDescargas");

                string basePath = connectionString;
                string folderName = DateTime.Now.ToString("dd MM yyyy");
                string outputPath = Path.Combine(basePath, folderName);
                string fileName = Path.GetFileName(inputFile);

                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                string outputFile = Path.Combine(outputPath, $"{obligacion}-{fileName}");

                if (startPage == 1)
                {
               
                
                    
                    if (File.Exists(outputFile))
                    {
                        enviarnotificacion = false;
                    }
                    else
                    {
                        File.Copy(inputFile, outputFile);
                        enviarnotificacion = true;
                    }
                }
                else
                {
                    try
                    {

                    
                    using (PdfReader reader = new PdfReader(inputFile))
                    {
                        int totalPages = reader.NumberOfPages;

                        if (startPage < 1 || startPage > totalPages)
                        {
                            Console.WriteLine("Error: La página de inicio está fuera del rango.");
                            return "0";
                        }

                        using (Document document = new Document())
                        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                        using (PdfCopy copy = new PdfCopy(document, fs))
                        {
                            document.Open();

                            for (int i = startPage; i <= totalPages; i++)
                            {
                                PdfImportedPage page = copy.GetImportedPage(reader, i);
                                if (page == null)
                                {
                                    Console.WriteLine($"No se pudo importar la página {i}.");
                                    return "0";
                                }

                                copy.AddPage(page);
                            }
                        }
                    }
                    }
                    catch (Exception ex)
                    {
                        if (File.Exists(outputFile))
                        {

                        }
                        else
                        {
                            File.Copy(inputFile, outputFile);
                        }
                        

                    }
                }
                if (enviarnotificacion == true)
                {
                    EnvioNotificaciones envio = new EnvioNotificaciones();
                    var enviarmail = envio.EnviarNotificacion(obligacion, outputFile);
                }
               
                return outputFile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return ex.Message;
            }
        }


    }
}
