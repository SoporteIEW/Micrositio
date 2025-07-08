using OpenQA.
    Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Micrositio.Sp;
using Micrositio.Funcionalidades;
using Micrositio.Modelos;
using System.Net;
using System.Net.Http;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
using System.Windows.Forms;

namespace Micrositio
{
    class Program
    {
        static IWebDriver m_driver;

        public class valdirecciones
        {

            public string idp { get; set; }

        }

        static async Task Main(string[] args)
        {
            try
            {
                List<Documentotabla> doc = new List<Documentotabla>();
                ProcedimientosAlmacenados sp = new ProcedimientosAlmacenados();
                List<string> prubatextos = new List<string>();
                //IWebDriver m_driver;
                //string path1 = AppDomain.CurrentDomain.BaseDirectory;
                //string direcccion = path1.Replace(@"\\", @"\");
                ////Iniciado = true;
                //m_driver = new ChromeDriver(direcccion);
                //m_driver.Manage().Window.Maximize();
                bool whilef = false;

                while (whilef == false)
                {
                    try
                    {                    
                    DateTime fechaUtilizar = DateTime.Now.AddDays(-4);
                    int canthilosf = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CantidadHilos"));
                    var procedimientos = new ProcedimientosAlmacenados();
                    List<RegistroSacosConsultar> registros = procedimientos.ObternerRegistroConsultar();
                    Parallel.ForEach(registros, new ParallelOptions { MaxDegreeOfParallelism = canthilosf }, itempf =>
                    {
                        string TIPOENTE2 = "";
                        TIPOENTE2 = itempf.TIPO_ENTE2;
                        TIPOENTE2 = TIPOENTE2.ToUpper();
                        string DPTO = "";
                        DPTO = itempf.DPTO_RADICACION;
                        string ciudad = "";
                        ciudad = itempf.CIUDAD_RADICACION;
                        string ciudadclick = "";
                        string departamentoclick = "";
                        string JuzdasoClick = "";
                        string anoclick = "";
                        string mesclick = "";
                        string mes = "";
                        string JUZGADON = "";
                        int id = 0;

                        string TIPOENTE = itempf.TIPO_ENTE;
                        TIPOENTE = TIPOENTE.ToUpper();

                        JUZGADON = itempf.JUZGADO;

                        id = itempf.ID;
                        List<ControlERncontradosprocesos> Listcont = new List<ControlERncontradosprocesos>();
                        var listaPorJuzgado = sp.ObternerListadoPorJuzgado(TIPOENTE2, JUZGADON, DPTO, ciudad);
                        foreach (var itemSpL in listaPorJuzgado)
                        {
                            Listcont.Add(new ControlERncontradosprocesos { digitos23 = itemSpL.NUMERO_PROCESO, Encontrado = false });
                        }
                        try
                        {
                            ChromeOptions options = new ChromeOptions();
                            // options.AddArgument("--headless"); // No abre ventana
                            options.AddArgument("--no-sandbox");
                            options.AddArgument("--disable-gpu");
                            options.AddArgument("--window-size=1920,1080");
                            options.AddArgument("--disable-blink-features=AutomationControlled");
                            IWebDriver m_driver = new ChromeDriver(options); // Aquí se abre, pero ya en modo headless

                            id = 0;
                            id = itempf.ID;
                            List<ListaDocumentos> listaDoc = new List<ListaDocumentos>();
                            SendKeys.SendWait("{SCROLLLOCK}");
                            try
                            {

                                m_driver.Url = "https://publicacionesprocesales.ramajudicial.gov.co/web/publicaciones-procesales";
                                var listadp = m_driver.FindElements(By.ClassName("col-md-3"));
                                foreach (var item in listadp)
                                {
                                    var elementosadentro = item.FindElements(By.XPath("./*"));
                                    foreach (var itema in elementosadentro)
                                    {
                                        string ide = itema.GetAttribute("id");
                                        if (ide.ToLower() == "departamento")
                                        {
                                            item.Click();
                                        }
                                    }
                                }
                                var buscador = m_driver.FindElement(By.ClassName("select2-search__field"));
                                DPTO = itempf.DPTO_RADICACION;
                                TIPOENTE = itempf.TIPO_ENTE;
                                TIPOENTE = TIPOENTE.ToUpper();
                                TIPOENTE2 = itempf.TIPO_ENTE2;
                                TIPOENTE2 = TIPOENTE2.ToUpper();
                                JUZGADON = itempf.JUZGADO;
                                ciudad = itempf.CIUDAD_RADICACION;
                                id = itempf.ID;
                                // string mes = DateTime.Now.AddMonths(-1).ToString("MMMM").ToLower();
                                mes = fechaUtilizar.ToString("MMMM").ToLower();
                                //buscador.SendKeys(DPTO);
                                buscador = m_driver.FindElement(By.ClassName("select2-search__field"));
                                string valortextDPTO = buscador.GetAttribute("value");
                                //while (valortextDPTO != DPTO)
                                //{
                                //    buscador = m_driver.FindElement(By.ClassName("select2-search__field"));
                                //    //buscador.SendKeys(DPTO);
                                //    valortextDPTO = buscador.GetAttribute("value");
                                //}
                                var valClick = m_driver.FindElement(By.ClassName("select2-results"));
                                var listadoDepartamentos = valClick.FindElements(By.TagName("li"));
                                if (listadoDepartamentos.Count > 1)
                                {
                                    foreach (var itemldc in listadoDepartamentos)
                                    {
                                        string NOMBREDPTO = itemldc.Text;
                                        if (NOMBREDPTO == DPTO)
                                        {
                                            itemldc.Click();
                                            break;
                                        }
                                    }
                                    departamentoclick = m_driver.FindElement(By.Id("select2-departamento-container")).Text;
                                }

                                listadp = m_driver.FindElements(By.ClassName("col-md-3"));
                                foreach (var item in listadp)
                                {
                                    var elementosadentro = item.FindElements(By.XPath("./*"));
                                    foreach (var itema in elementosadentro)
                                    {
                                        string ide = itema.GetAttribute("id");
                                        if (ide.ToLower() == "municipio")
                                        {
                                            item.Click();
                                        }
                                    }
                                }

                                buscador = m_driver.FindElement(By.ClassName("select2-search__field"));

                                buscador.SendKeys(ciudad);
                                buscador = m_driver.FindElement(By.ClassName("select2-search__field"));
                                string valortextciudad = buscador.GetAttribute("value");
                                while (valortextciudad != ciudad)
                                {
                                    buscador = m_driver.FindElement(By.ClassName("select2-search__field"));
                                    buscador.SendKeys(ciudad);
                                    valortextciudad = buscador.GetAttribute("value");
                                }
                                var valClickC = m_driver.FindElement(By.ClassName("select2-results"));
                                var listadoCiudades = valClickC.FindElements(By.TagName("li"));
                                if (listadoCiudades.Count > 0)
                                {
                                    listadoCiudades[0].Click();
                                    ciudadclick = m_driver.FindElement(By.Id("select2-municipio-container")).Text;
                                }

                                listadp = m_driver.FindElements(By.ClassName("col-md-3"));
                                foreach (var item in listadp)
                                {
                                    var elementosadentro = item.FindElements(By.XPath("./*"));
                                    foreach (var itema in elementosadentro)
                                    {
                                        string ide = itema.GetAttribute("id");
                                        if (ide.ToLower() == "despacho")
                                        {
                                            item.Click();
                                        }

                                    }


                                }


                                var valClickJuz = m_driver.FindElement(By.ClassName("select2-results"));
                                var listadoJuzgados = valClickJuz.FindElements(By.TagName("li"));
                                string[] palabrasDespacho = TIPOENTE.Split(new char[] { ' ', ',', '.', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);


                                foreach (var item in listadoJuzgados)
                                {
                                    string itentext = item.Text;
                                    bool contieneTodas = palabrasDespacho.All(palabra => itentext.IndexOf(palabra, StringComparison.OrdinalIgnoreCase) >= 0);

                                    if (contieneTodas == true)
                                    {
                                        if (itentext.IndexOf(JUZGADON) >= 0)
                                        {
                                            item.Click();
                                            JuzdasoClick = m_driver.FindElement(By.Id("select2-despacho-container")).Text;
                                            break;
                                        }
                                    }

                                }

                                listadp = m_driver.FindElements(By.ClassName("col-md-3"));
                                foreach (var item in listadp)
                                {
                                    var elementosadentro = item.FindElements(By.XPath("./*"));
                                    foreach (var itema in elementosadentro)
                                    {
                                        string ide = itema.GetAttribute("id");
                                        if (ide.ToLower() == "anno")
                                        {
                                            item.Click();
                                        }

                                    }


                                }



                                var valClickano = m_driver.FindElement(By.ClassName("select2-results"));
                                var listadoanos = valClickano.FindElements(By.TagName("li"));
                                foreach (var item in listadoanos)
                                {
                                    if (item.Text == fechaUtilizar.ToString("yyyy"))
                                    {
                                        item.Click();
                                        anoclick = m_driver.FindElement(By.Id("select2-anno-container")).Text;
                                        break;
                                    }
                                }
                                listadp = m_driver.FindElements(By.ClassName("col-md-3"));
                                foreach (var item in listadp)
                                {
                                    var elementosadentro = item.FindElements(By.XPath("./*"));
                                    foreach (var itema in elementosadentro)
                                    {
                                        string ide = itema.GetAttribute("id");
                                        if (ide.ToLower() == "mes")
                                        {
                                            item.Click();
                                        }

                                    }


                                }
                                var valclickmeses = m_driver.FindElement(By.ClassName("select2-results"));
                                var listadomeses = valclickmeses.FindElements(By.TagName("li"));
                                foreach (var item in listadomeses)
                                {

                                    string mesittemlist = item.Text.ToLower();
                                    if (mesittemlist.IndexOf(mes) >= 0)
                                    {
                                        item.Click();
                                        mesclick = m_driver.FindElement(By.Id("select2-mes-container")).Text;
                                        break;
                                    }
                                }

                                var buttonBuscar = m_driver.FindElement(By.Id("buscar"));
                                buttonBuscar.Click();




                                if (ciudadclick != "" && departamentoclick != "" && JuzdasoClick != "" && anoclick != "" && mesclick != "" && ciudadclick != "Todos")
                                {

                                    string rutaProyectoe = AppDomain.CurrentDomain.BaseDirectory;
                                    string carpetaDescargase = Path.Combine(rutaProyectoe, "Descargas");
                                    string[] archivos = Directory.GetFiles(carpetaDescargase);

                                    foreach (string archivo in archivos)
                                    {
                                        File.Delete(archivo);
                                    }

                                    List<string> DireccionesDocumentos = new List<string>();
                                    List<valdirecciones> DireccionesDocumentosid = new List<valdirecciones>();

                                    bool whilepaginacion = true;

                                    while (whilepaginacion)
                                    {
                                        var contendorPublicaciones = m_driver.FindElement(By.Id("_co_com_avanti_efectosProcesales_PublicacionesEfectosProcesalesPortlet_INSTANCE_qOzzZevqIWbb_publicacionesVOsSearchContainer"));
                                        var valrerro = m_driver.FindElements(By.Id("_co_com_avanti_efectosProcesales_PublicacionesEfectosProcesalesPortlet_INSTANCE_qOzzZevqIWbb_publicacionesVOsSearchContainerEmptyResultsMessage"));

                                        if (valrerro.Count == 1)
                                        {
                                            bool esEsteMesYAnio = false;

                                            var listadopublicaciones = contendorPublicaciones.FindElements(By.TagName("tr"));
                                            foreach (var item in listadopublicaciones)
                                            {
                                                var elementfp = item.FindElements(By.TagName("p"));
                                                foreach (var itemFP in elementfp)
                                                {
                                                    string fechapublicacion = itemFP.Text;

                                                    if (fechapublicacion.IndexOf("Fecha de Publicación:") >= 0)
                                                    {
                                                        var match = Regex.Match(fechapublicacion, @"\d{4}-\d{2}-\d{2}");
                                                        if (match.Success)
                                                        {
                                                            DateTime fechaExtraida = DateTime.Parse(match.Value);
                                                            DateTime fechaActual = fechaUtilizar;

                                                            esEsteMesYAnio =
                                                               fechaExtraida.Month == fechaActual.Month &&
                                                               fechaExtraida.Year == fechaActual.Year;
                                                            break;
                                                        }
                                                    }
                                                }


                                                if (esEsteMesYAnio == true)
                                                {

                                                    var element = item.FindElements(By.TagName("a"));

                                                    foreach (var item2 in element)
                                                    {
                                                        string valora = item2.Text.ToLower();
                                                        if (valora == "ver detalle")
                                                        {
                                                            string dirvinculop = item2.GetAttribute("href");
                                                            string articleId = "";
                                                            var match = Regex.Match(dirvinculop, @"articleId=(\d+)");
                                                            if (match.Success)
                                                            {
                                                                articleId = match.Groups[1].Value;
                                                            }

                                                            var valexistl = DireccionesDocumentosid.Where(x => x.idp == articleId).ToList();
                                                            if (valexistl.Count == 0)
                                                            {
                                                                DireccionesDocumentos.Add(dirvinculop);
                                                                DireccionesDocumentosid.Add(new valdirecciones { idp = articleId });
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                whilepaginacion = false;
                                                            }

                                                        }
                                                    }
                                                }
                                            }

                                            if (listadopublicaciones.Count <= 0)
                                            {
                                                whilepaginacion = false;
                                            }
                                            var paginacionClass = m_driver.FindElement(By.ClassName("lfr-pagination-buttons"));
                                            var listaPaginacion = paginacionClass.FindElements(By.TagName("li"));
                                            foreach (var item in listaPaginacion)
                                            {
                                                if (item.Text.ToLower() == "siguiente")
                                                {
                                                    if (item.GetAttribute("class") == "disabled")
                                                    {
                                                        whilepaginacion = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        item.Click();
                                                        contendorPublicaciones = m_driver.FindElement(By.Id("_co_com_avanti_efectosProcesales_PublicacionesEfectosProcesalesPortlet_INSTANCE_qOzzZevqIWbb_publicacionesVOsSearchContainer"));
                                                        var validarSiguiente = contendorPublicaciones.FindElements(By.ClassName("row"));
                                                        if (validarSiguiente.Count <= 0)
                                                        {
                                                            whilepaginacion = false;
                                                        }

                                                        break;
                                                    }
                                                }

                                            }

                                        }
                                        else
                                        {
                                            whilepaginacion = false;
                                        }

                                    }
                                    foreach (var item in DireccionesDocumentos)
                                    {

                                        //var ValidaranoP = m_driver.FindElements(By.TagName("td"));
                                        //foreach (var itemBp in ValidaranoP)
                                        //{

                                        //}

                                        bool whiledocl = true;

                                        m_driver.Url = item;
                                        string idcarpeta = "";

                                        var match2 = Regex.Match(item, @"articleId=(\d+)");
                                        if (match2.Success)
                                        {
                                            idcarpeta = match2.Groups[1].Value;
                                        }

                                        var identificarCarpeta = m_driver.FindElements(By.TagName("h4"));
                                        foreach (var item4 in identificarCarpeta)
                                        {
                                            string valid = item4.Text;
                                            if (valid.IndexOf("ID") >= 0)
                                            {

                                                Regex regex = new Regex(@"ID Carpeta\s+(\d+)");
                                                Match match = regex.Match(valid);
                                                idcarpeta = match.Groups[1].Value;
                                                prubatextos.Add(idcarpeta);

                                            }

                                        }


                                        while (whiledocl)
                                        {




                                            var divtable = m_driver.FindElement(By.ClassName("docs-publicacion"));

                                            var filastabla = divtable.FindElements(By.TagName("tr"));

                                            foreach (var item2 in filastabla)
                                            {

                                                var colimnas = item2.FindElements(By.TagName("td"));

                                                string nombreD = "";
                                                string urlD = "";
                                                string fechaD = "";
                                                if (colimnas.Count == 2)
                                                {


                                                    for (int i = 0; i < colimnas.Count; i++)
                                                    {


                                                        if (i == 0)
                                                        {
                                                            var buscarruta = colimnas[i].FindElement(By.TagName("a"));

                                                            urlD = buscarruta.GetAttribute("href"); ;
                                                            nombreD = colimnas[i].Text;
                                                        }
                                                        if (i == 1)
                                                        {
                                                            fechaD = colimnas[i].Text;
                                                            listaDoc.Add(new ListaDocumentos
                                                            {
                                                                NombreDoc = nombreD.ToLower(),
                                                                FechaDoc = fechaD,
                                                                UrlDoc = urlD,
                                                                idcarpeda = idcarpeta,
                                                                urlcarpeda = item

                                                            });
                                                        }

                                                    }

                                                }

                                            }

                                            var paginacionarchivos = m_driver.FindElement(By.ClassName("dataTables_paginate"));
                                            var listaA = paginacionarchivos.FindElements(By.TagName("a"));
                                            foreach (var item3 in listaA)
                                            {
                                                if (item3.Text == "Siguiente")
                                                {
                                                    if (item3.GetAttribute("tabindex") == "-1")
                                                    {
                                                        whiledocl = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            item3.Click();
                                                        }
                                                        catch (Exception)
                                                        {
                                                            whiledocl = false;
                                                            break;

                                                        }


                                                    }
                                                }
                                            }

                                        }


                                    }

                                    List<formasnumero> listabuscarT = new List<formasnumero>();


                                    foreach (var item in listaPorJuzgado)
                                    {
                                        string registroadd = item.NUMERO_PROCESO;
                                        if (registroadd.Length == 23)
                                        {
                                            listabuscarT.Add(new formasnumero { digitos23 = registroadd, Forma = registroadd, Encontrado = false });
                                            string Numero1 = registroadd.Substring(12, 4) + " " + registroadd.Substring(16, 5);
                                            listabuscarT.Add(new formasnumero { digitos23 = registroadd, Forma = Numero1, Encontrado = false });
                                            listabuscarT.Add(new formasnumero { digitos23 = registroadd, Forma = Numero1.Replace(" ", ""), Encontrado = false });
                                            string Numero2 = registroadd.Substring(12, 4) + " " + registroadd.Substring(16, 5);
                                            listabuscarT.Add(new formasnumero { digitos23 = registroadd, Forma = Numero2, Encontrado = false });
                                            listabuscarT.Add(new formasnumero { digitos23 = registroadd, Forma = Numero2.Replace(" ", ""), Encontrado = false });
                                            string Numero3 = registroadd.Substring(12, 4) + " " + registroadd.Substring(16, 5) + " " + registroadd.Substring(registroadd.Length - 2);
                                            listabuscarT.Add(new formasnumero { digitos23 = registroadd, Forma = Numero3, Encontrado = false });
                                            listabuscarT.Add(new formasnumero { digitos23 = registroadd, Forma = Numero3.Replace(" ", ""), Encontrado = false });
                                        }
                                    }
                                    List<string> listabuscar = new List<string>();
                                    foreach (var item in listabuscarT)
                                    {
                                        listabuscar.Add(item.Forma);
                                    }


                                    //var ppp = listabuscar.Where(x => x == "73001400300220240035600").ToList();

                                    var agrupados = listaDoc
                             .GroupBy(d => new { d.idcarpeda, d.urlcarpeda }) // Agrupar por idcarpeda y UrlDoc
                             .Select(g => new
                             {
                                 idcarpeda = g.Key.idcarpeda,
                                 CantidadDocumentos = g.Count(),
                                 urlcarpeda = g.Key.urlcarpeda
                             })
                             .ToList();



                                    List<Carpetasencontradas> carpetasencontradas = new List<Carpetasencontradas>();

                                    foreach (var itemcarpetasencontadas in listaDoc)
                                    {
                                        var valcarpl = carpetasencontradas.Where(x => x.id == itemcarpetasencontadas.idcarpeda).FirstOrDefault();
                                        if (valcarpl == null)
                                        {
                                            carpetasencontradas.Add(new Carpetasencontradas { id = itemcarpetasencontadas.idcarpeda, url = itemcarpetasencontadas.urlcarpeda });
                                        }
                                    }

                                    Parallel.ForEach(carpetasencontradas, new ParallelOptions { MaxDegreeOfParallelism = 12 }, item =>
                                    {

                                        //   foreach (var item in carpetasencontradas)
                                        //{
                                        var consultarcarpera = sp.ConsultaPorCarpetaMicrositio(item.id, item.url);
                                        if (consultarcarpera.Count == 0)
                                        {



                                            var buscarid = listaDoc.Where(x => x.idcarpeda == item.id).ToList();

                                            foreach (var itemListDoc in buscarid)
                                            {
                                                List<DocumentosDescargados> ArchivosDescargados = new List<DocumentosDescargados>();
                                                Parallel.ForEach(buscarid, new ParallelOptions { MaxDegreeOfParallelism = 12 }, item2 =>
                                                {

                                                //        foreach (var item2 in listselect)
                                                //{

                                                string rutaProyecto = AppDomain.CurrentDomain.BaseDirectory;
                                                    string carpetaDescargas = Path.Combine(rutaProyecto, "Descargas");
                                                    string archivoDestino = Path.Combine(carpetaDescargas, item2.NombreDoc);
                                                    string url = item2.UrlDoc;
                                                    if (!Directory.Exists(carpetaDescargas))
                                                    {
                                                        Directory.CreateDirectory(carpetaDescargas);
                                                    }

                                                    using (WebClient client = new WebClient())
                                                    {
                                                        try
                                                        {
                                                            client.DownloadFile(url, archivoDestino);
                                                            Console.WriteLine("Archivo descargado en: " + archivoDestino);
                                                            ArchivosDescargados.Add(new DocumentosDescargados { rutaDescarga = archivoDestino, FechaDoc = item2.FechaDoc, NombreDoc = item2.NombreDoc, UrlDoc = item2.UrlDoc, urlCarpeta = item2.urlcarpeda });
                                                        }
                                                        catch (Exception)
                                                        {
                                                        }

                                                    }
                                                });

                                                //volver paraell
                                                foreach (var itemBuscarazure in ArchivosDescargados)
                                                {
                                                    funcionesAzure funcionesAzure = new funcionesAzure();
                                                    var resultazure = funcionesAzure.BuscarPalabrasEnPdf(itemBuscarazure.rutaDescarga, listabuscar);
                                                    if (resultazure.Count > 0)
                                                    {
                                                        foreach (var itemrazure in resultazure)
                                                        {
                                                            var buscarEncontrado = listabuscarT.Where(x => x.Forma == itemrazure.obligacion).FirstOrDefault();
                                                            var listapupdate = Listcont.Where(x => x.digitos23 == buscarEncontrado.digitos23).FirstOrDefault();
                                                            listapupdate.Encontrado = true;
                                                        }
                                                    }


                                                    List<dynamic> validacionvaloresazure = new List<dynamic>();
                                                    foreach (var listabuscarTazure in resultazure)
                                                    {
                                                        var valazure = listabuscarT.Where(x => x.Forma == listabuscarTazure.obligacion).FirstOrDefault();

                                                        if (validacionvaloresazure.Count == 0)
                                                        {
                                                            validacionvaloresazure.Add(new { pagina = listabuscarTazure.pagina, obligacion = listabuscarTazure.obligacion, padreobligacion = valazure.digitos23 });
                                                        }
                                                        else
                                                        {
                                                            var valenlistn = validacionvaloresazure.Where(x => x.pagina == listabuscarTazure.pagina && x.padreobligacion == valazure.digitos23).FirstOrDefault();
                                                            if (valenlistn == null)
                                                            {
                                                                validacionvaloresazure.Add(new { pagina = listabuscarTazure.pagina, obligacion = listabuscarTazure.obligacion, padreobligacion = valazure.digitos23 });
                                                            }
                                                        }
                                                    }


                                                    if (validacionvaloresazure.Count > 0)
                                                    {
                                                        foreach (var ListResultAzure in validacionvaloresazure)
                                                        {

                                                            GuardarPieza guardarPieza = new GuardarPieza();
                                                            string guardarPiezaResult = guardarPieza.GuardarPiezas(itemBuscarazure.rutaDescarga, ListResultAzure.pagina, ListResultAzure.obligacion);

                                                            string fechadocumento = itemBuscarazure.FechaDoc;
                                                            DateTime fechaConvertida;
                                                            if (ConvertirFecha(itemBuscarazure.FechaDoc, out fechaConvertida))
                                                            {
                                                                fechadocumento = fechaConvertida.ToString("yyyy-MM-dd HH:mm:ss");
                                                            }

                                                            ProcedimientosAlmacenados insert = new ProcedimientosAlmacenados();
                                                            var insertregistro = insert.InsertRutaPrincipal(ListResultAzure.padreobligacion, fechadocumento, guardarPiezaResult, item.url, itemBuscarazure.UrlDoc, ListResultAzure.obligacion, itemBuscarazure.NombreDoc);
                                                        }

                                                    }

                                                }
                                            }

                                            var insertarconsultam = sp.InsertarConsultasPorCarpetaMicrositio(item.id, item.url, DateTime.Now, TIPOENTE2, ciudad, DPTO, DateTime.Now.ToString("yyyy"), mes, false, "", JUZGADON, id);
                                            // aqui guardae log carpeta 
                                        }
                                    });

                                    if (ciudadclick == "")
                                    {
                                        ciudadclick = ciudad + " error";
                                    }
                                    if (departamentoclick == "")
                                    {
                                        departamentoclick = DPTO + " error";
                                    }
                                    if (JuzdasoClick == "")
                                    {
                                        JuzdasoClick = TIPOENTE + " " + TIPOENTE2 + " error";
                                    }
                                    if (anoclick == "")
                                    {
                                        anoclick = DateTime.Now.ToString("yyyy") + " error";
                                    }
                                    if (mesclick == "")
                                    {
                                        mesclick = DateTime.Now.ToString("MMMM") + " error";
                                    }

                                    foreach (var itemvalencp in Listcont)
                                    {
                                        var insertarogBUsqueda = sp.InsertarProcesosBusqueda(itemvalencp.digitos23, itemvalencp.Encontrado, ciudadclick, departamentoclick, anoclick, mesclick, JuzdasoClick);
                                    }
                                    var actualizar = sp.actualizarfechaconsultaJuzgado(id);

                                }
                                else
                                {
                                    if (ciudadclick == "")
                                    {
                                        ciudadclick = ciudad + " error";
                                    }
                                    if (departamentoclick == "")
                                    {
                                        departamentoclick = DPTO + " error";
                                    }
                                    if (JuzdasoClick == "")
                                    {
                                        JuzdasoClick = TIPOENTE + " " + TIPOENTE2 + " error";
                                    }
                                    if (anoclick == "")
                                    {
                                        anoclick = DateTime.Now.ToString("yyyy") + " error";
                                    }
                                    if (mesclick == "")
                                    {
                                        mesclick = DateTime.Now.ToString("MMMM") + " error";
                                    }
                              
                                   

                                    foreach (var itemvalencp in Listcont)
                                    {
                                        var insertarogBUsqueda = sp.InsertarProcesosBusqueda(itemvalencp.digitos23, itemvalencp.Encontrado, ciudadclick, departamentoclick, anoclick, mesclick, JuzdasoClick);
                                    }
                                    ProcedimientosAlmacenados sps2 = new ProcedimientosAlmacenados();
                                    var insertarconsultam2 = sps2.InsertarConsultasPorCarpetaMicrositio("N/A", "N/A", DateTime.Now, TIPOENTE2, ciudad, DPTO, DateTime.Now.ToString("yyyy"), mes, true, "Error En campos", JUZGADON, id);
                                    var actualizar = sp.actualizarfechaconsultaJuzgado(id);
                                }
                                m_driver.Close();
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    m_driver.Close();
                                    ProcedimientosAlmacenados sps2 = new ProcedimientosAlmacenados();
                                    foreach (var itemvalencp in Listcont)
                                    {
                                        var insertarogBUsqueda = sp.InsertarProcesosBusqueda(itemvalencp.digitos23, itemvalencp.Encontrado, ciudadclick, departamentoclick, anoclick, mesclick, JuzdasoClick);
                                    }
                                    var insertarconsultam2 = sps2.InsertarConsultasPorCarpetaMicrositio("N/A", "N/A", DateTime.Now, TIPOENTE2, ciudad, DPTO, DateTime.Now.ToString("yyyy"), mes, true, ex.Message, JUZGADON, id);
                                    var actualizar = sp.actualizarfechaconsultaJuzgado(id);
                                }
                                catch (Exception)
                                {
                                    m_driver.Close();


                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            if (ciudadclick == "")
                            {
                                ciudadclick = ciudad + " error";
                            }
                            if (departamentoclick == "")
                            {
                                departamentoclick = DPTO + " error";
                            }
                            if (JuzdasoClick == "")
                            {
                                JuzdasoClick = TIPOENTE + " " + TIPOENTE2 + " error";
                            }
                            if (anoclick == "")
                            {
                                anoclick = DateTime.Now.ToString("yyyy") + " error";
                            }
                            if (mesclick == "")
                            {
                                mesclick = DateTime.Now.ToString("MMMM") + " error";
                            }
                            m_driver.Close();
                            ProcedimientosAlmacenados sps2 = new ProcedimientosAlmacenados();
                            var insertarconsultam2 = sps2.InsertarConsultasPorCarpetaMicrositio("N/A", "N/A", DateTime.Now, TIPOENTE2, ciudad, DPTO, DateTime.Now.ToString("yyyy"), "N/A", true, ex.Message, "N/A", id);
                            foreach (var itemvalencp in Listcont)
                            {
                                var insertarogBUsqueda = sp.InsertarProcesosBusqueda(itemvalencp.digitos23, itemvalencp.Encontrado, ciudadclick, departamentoclick, anoclick, mesclick, JuzdasoClick);
                            }
                            var actualizar = sp.actualizarfechaconsultaJuzgado(id);
                        }

                    });
                    }
                    catch (Exception)
                    {                
                    }

                }



            }
            catch (Exception)
            {

            }

        }
        static bool ConvertirFecha(string fecha, out DateTime fechaSalida)
        {
            try
            {

           
            string formato = "dd-MMM-yyyy HH:mm:ss"; // Formato esperado
            CultureInfo cultura = new CultureInfo("es-ES", false);
            //--// Asegurar que los nombres de los meses sean interpretados correctamente
            cultura.DateTimeFormat.AbbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
            cultura.DateTimeFormat.AbbreviatedMonthGenitiveNames = cultura.DateTimeFormat.AbbreviatedMonthNames;
            return DateTime.TryParseExact(fecha, formato, cultura, DateTimeStyles.None, out fechaSalida);
            }
            catch (Exception)
            {
                string formato = "dd-MMM-yyyy HH:mm:ss"; // Formato esperado
                CultureInfo cultura = new CultureInfo("es-ES", false);
                //--// Asegurar que los nombres de los meses sean interpretados correctamente
                cultura.DateTimeFormat.AbbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
                cultura.DateTimeFormat.AbbreviatedMonthGenitiveNames = cultura.DateTimeFormat.AbbreviatedMonthNames;
                return DateTime.TryParseExact(DateTime.Now.ToString("dd-MMM-yyyy HH:mm").ToLower(), formato, cultura, DateTimeStyles.None, out fechaSalida);

            }
        }



        public class ListaDocumentos
        {
            public string NombreDoc { get; set; }
            public string FechaDoc { get; set; }
            public string UrlDoc { get; set; }
            public string idcarpeda { get; set; }
            public string urlcarpeda { get; set; }
        }

        public class DocumentosDescargados
        {
            public string rutaDescarga { get; set; }
            public string FechaDoc { get; set; }
            public string NombreDoc { get; set; }
            public string UrlDoc { get; set; }
            public string urlCarpeta { get; set; }
        }

        public class formasnumero
        {
            public string digitos23 { get; set; }
            public string Forma { get; set; }
            public bool Encontrado { get; set; }

        }

        public class ControlERncontradosprocesos
        {
            public string digitos23 { get; set; }
            public bool Encontrado { get; set; }

        }


        public class Carpetasencontradas
        {
            public string id { get; set; }
            public string url { get; set; }

        }

    }
}
