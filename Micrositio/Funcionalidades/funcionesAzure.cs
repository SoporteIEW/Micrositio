using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using static Micrositio.Funcionalidades.funcionesAzure;

namespace Micrositio.Funcionalidades
{
  public  class funcionesAzure
    {
        public class retorno {
            public string  obligacion  { get; set; }
            public int pagina { get; set; }
        }


        public dynamic BuscarPalabrasEnPdf(string pdfPath, List<string> palabrasClave)
        {
            List<retorno> respuesta = new List<retorno>();

            try
            {
                using (PdfReader reader = new PdfReader(pdfPath))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string textoPagina = PdfTextExtractor.GetTextFromPage(reader, i).ToLower();
                        Console.WriteLine($"Texto extraído en página {i}: {textoPagina}");
                        textoPagina = textoPagina.Replace("-", "");
                        foreach (var palabra in palabrasClave)
                        {
                            if (textoPagina.Contains(palabra.ToLower()))
                            {
                                respuesta.Add(new retorno { obligacion = palabra, pagina = i });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return respuesta;
        }

        //public async Task<dynamic> BuscarPalabrasEnPdf(string pdfPath, List<string> palabrasClave)
        //{
        //    List<retorno> respuesta = new List<retorno>();

        //    try
        //    {
        //        using (PdfReader reader = new PdfReader(pdfPath))
        //        {
        //            for (int i = 1; i <= reader.NumberOfPages; i++)
        //            {
        //                string textoPagina = PdfTextExtractor.GetTextFromPage(reader, i).ToLower();
        //                Console.WriteLine($"Texto extraído en página {i}: {textoPagina}");
        //                textoPagina = textoPagina.Replace("-", "");
        //                foreach (var palabra in palabrasClave)
        //                {
        //                    if (textoPagina.Contains(palabra.ToLower()))
        //                    {
        //                        respuesta.Add(new retorno { obligacion = palabra, pagina = i });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }

        //    return respuesta;
        //}


        //public async Task<dynamic> BuscarPalabrasEnPdf(string pdfPath, List<string> palabrasClave)
        //{
        //    List<retorno> respuesta = new List<retorno>();

        //    string endpoint = "https://micrositiobusquedadoccac.cognitiveservices.azure.com/";
        //    string apiKey = "2INKHhfZ2bKLy6EQJyszokyZXmYfwAoCTyyTzrCjrODxOOWLRB5uJQQJ99BBACLArgHXJ3w3AAALACOGjXrm";

        //    var resultado = await BuscarTextoEnPdfAzure(endpoint, apiKey, pdfPath, palabrasClave);

        //    foreach (var palabra in resultado)
        //    {
        //        if (palabra.Value.Count > 0)
        //        {
        //            int pagina = palabra.Value[0];
        //            respuesta.Add(new retorno { obligacion = palabra.Key, pagina = pagina });
        //        }
        //        else {
        //            Console.WriteLine($"Palabra '{palabra.Key}' no encontrada.");
        //        }

        //    }

        //    return respuesta;
        //}

        //static async Task<Dictionary<string, List<int>>> BuscarTextoEnPdfAzure(string endpoint, string apiKey, string pdfPath, List<string> palabrasClave)
        //{
        //    var client = new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        //    Dictionary<string, List<int>> palabrasEncontradas = new Dictionary<string, List<int>>();
        //    FileStream stream = null;

        //    try
        //    {
        //        stream = File.OpenRead(pdfPath);
        //        AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", stream);
        //        AnalyzeResult result = operation.Value;

        //        foreach (string palabra in palabrasClave)
        //        {
        //            palabrasEncontradas[palabra] = new List<int>();
        //        }

        //        foreach (var page in result.Pages)
        //        {
        //            // 🔹 Extrae el texto correctamente, eliminando espacios
        //            string textoPagina = string.Join(" ", page.Words.Select(w => w.Content)).ToLower().Replace("-", "");
        //            Console.WriteLine($"Texto extraído en página {page.PageNumber}: {textoPagina}");

        //            foreach (var palabra in palabrasClave)
        //            {
        //                // 🔹 Normaliza la palabra clave (quita espacios)
        //                string palabraNormalizada = palabra;

        //                if (textoPagina.Contains(palabraNormalizada))
        //                {
        //                    palabrasEncontradas[palabra].Add(page.PageNumber);
        //                }
        //            }
        //        }

        //        return palabrasEncontradas;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return palabrasEncontradas;
        //    }
        //    finally
        //    {
        //        stream?.Dispose(); // Cierra el archivo
        //    }
        //}


    }
}
