using System;
using System.Linq;
using Telerik.Reporting.Processing;
using _DAO.DAO;
using _Model;
using _Model.Entities;
using System.Configuration;
using System.Collections.Generic;
using _Rules._WsVecinoVirtual;
using System.Web;
using System.Data;
using System.IO;

namespace _Rules.Rules
{
    public class Rules_Reportes : BaseRules<Reporte>
    {
        private readonly DAO_Reportes dao;
        private readonly Rules_Inscripcion _InscripcionRules;

        public Rules_Reportes(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_Reportes.Instance;
            _InscripcionRules = new Rules_Inscripcion(data);
        }
        public Resultado<TipoAuto> GetByKeyValue(Enums.TipoAuto keyValue)
        {
            return dao.GetByKeyValue(keyValue);
        }


        public Resultado<string> GetInscripcionesPorDni(int? dni)
        {           
            var resultado = new Resultado<string>();
            var consulta = new _Model.Consultas.Consulta_Inscripcion();
            if (dni.HasValue)
            {
                consulta.Dni = dni;
            }
            else
            {
                resultado.Error = "Ingrese un DNI valido";
                return resultado;
            }
                
            
            var resultadoInscripcion = _InscripcionRules.Get(consulta);
            if (!resultadoInscripcion.Ok)
            {
                resultado.Error = resultadoInscripcion.Error;
                return resultado;
            }
            var inscripciones = resultadoInscripcion.Return;

            if (inscripciones.Count == 0)
            {
                resultado.Error = "No se encontraron inscripciones con los filtros ingresados";
                return resultado;
            }

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Reporte_Inscripcion_Dni_encabezado.trdx");
                //var rutaReportePadre = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.FullName;

                //var rutaReporte = rutaReportePadre + "/_Rules/Resources/Reportes/Reporte_Inscripcion_Dni_encabezado.trdx";

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                System.Xml.XmlReader xmlReaderRequerimiento = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerRequerimiento = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerRequerimiento.Deserialize(xmlReaderRequerimiento);

                //---------------------------------
                // Defino el DT
                //---------------------------------

                Telerik.Reporting.ObjectDataSource objectDataSourceInscripcion = new Telerik.Reporting.ObjectDataSource();
                DataTable dtInscripcionUsuario = new DataTable();

                //Campos
                dtInscripcionUsuario.Columns.Add("Nombre");
                dtInscripcionUsuario.Columns.Add("Apellido");
                dtInscripcionUsuario.Columns.Add("Documento");
                dtInscripcionUsuario.Columns.Add("DomicilioCalle");
                dtInscripcionUsuario.Columns.Add("DomicilioAltura");
                dtInscripcionUsuario.Columns.Add("DomicilioBarrio");




                //-------------------------------------
                // Creo una fila y le cargo los datos
                //-------------------------------------

                DataRow filaInscripcion = dtInscripcionUsuario.NewRow();

                //****************Encabezado*********************************
                //****************USUARIO********************************
                var usuario = inscripciones[0].Usuario;
                if (usuario == null)
                {
                    resultado.Error = "No se encontro el usuario";
                    return resultado;
                }
               
              
                
                //Nombre
                string nombre = usuario.Nombre;
                if (string.IsNullOrEmpty(nombre))
                {
                    nombre = "Sin Datos";
                }
                filaInscripcion["Nombre"] = nombre;

                //Apellido
                string apellido = usuario.Apellido;
                if (string.IsNullOrEmpty(apellido))
                {
                    apellido = "Sin Datos";
                }
                filaInscripcion["Apellido"] = apellido;

                //Dni
                string documento = usuario.Dni.ToString();
                if (string.IsNullOrEmpty(documento))
                {
                    documento = "Sin Datos";
                }
                filaInscripcion["Documento"] = dni;

                //Calle
                string calle = usuario.DomicilioCalle;
                if (string.IsNullOrEmpty(calle))
                {
                    calle = "Sin Datos";
                }
                filaInscripcion["DomicilioCalle"] = calle;
                //Altura
                string altura = usuario.DomicilioAltura.ToString();
                if (string.IsNullOrEmpty(altura))
                {
                    altura = "Sin Datos";
                }
                filaInscripcion["DomicilioAltura"] = altura;
                //Barrio
                string barrio = usuario.DomicilioBarrio;
                if (string.IsNullOrEmpty(barrio))
                {
                    barrio = "Sin Datos";
                }
                filaInscripcion["DomicilioBarrio"] = barrio;


                Telerik.Reporting.SubReport subReportItem = (Telerik.Reporting.SubReport)(reporte.Items.Find("subreporteInsripciones", true)[0]);
                subReportItem.ReportSource = crearSubreporteInscripcionesDni(inscripciones).Return;

                //Agrego la fila del requerimiento
                dtInscripcionUsuario.Rows.Add(filaInscripcion);

                //Seteo el datasource al reclamo
                objectDataSourceInscripcion.DataSource = dtInscripcionUsuario;
                reporte.DataSource = objectDataSourceInscripcion;

                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
                resultado.Return = base64;

            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            return resultado;
        }

        public Resultado<Telerik.Reporting.Report> crearSubreporteInscripcionesDni(List<Inscripcion> inscripciones)
        {
            var result = new Resultado<Telerik.Reporting.Report>();

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Reporte_Inscripcion_Dni_detalle.trdx");
                //var rutaReportePadre = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.FullName;

                //var rutaReporte = rutaReportePadre + "/_Rules/Resources/Reportes/Reporte_Inscripcion_Dni_detalle.trdx";

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                System.Xml.XmlReader xmlReaderRequerimiento = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerRequerimiento = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerRequerimiento.Deserialize(xmlReaderRequerimiento);

                //---------------------------------
                // Defino el DT
                //---------------------------------

                Telerik.Reporting.ObjectDataSource objectDataSourceInscripcion = new Telerik.Reporting.ObjectDataSource();
                DataTable dtInscripcion = new DataTable();

                //Campos
                dtInscripcion.Columns.Add("TipoAuto");
                dtInscripcion.Columns.Add("idTipoAuto");
                dtInscripcion.Columns.Add("identificador");
                dtInscripcion.Columns.Add("FechaInicio");
                dtInscripcion.Columns.Add("FechaFin");
                dtInscripcion.Columns.Add("FechaTelegrama");

                foreach (var i in inscripciones)
                {
                    //-------------------------------------
                    // Creo una fila y le cargo los datos
                    //-------------------------------------

                    DataRow filaInscripcion = dtInscripcion.NewRow();

                    filaInscripcion["TipoAuto"] = i.TipoAuto.Nombre;
                    filaInscripcion["idTipoAuto"] = i.TipoAuto.Id;
                    filaInscripcion["identificador"] = i.Identificador;



                    var fechaInicio = i.FechaInicio;
                    if (!string.IsNullOrEmpty(fechaInicio.ToString()))
                    {
                        filaInscripcion["FechaInicio"] = i.FechaInicio.Value.ToString("dd/MM/yyyy");
                    }

                    var fechaFin = i.FechaFin;
                    if (!string.IsNullOrEmpty(fechaFin.ToString()))
                    {
                        filaInscripcion["FechaFin"] = i.FechaFin.Value.ToString("dd/MM/yyyy");
                    }

                    var fechaTelegrama = i.FechaTelegrama;
                    if (!string.IsNullOrEmpty(fechaTelegrama.ToString()))
                    {
                        filaInscripcion["FechaTelegrama"] = fechaTelegrama.Value.ToString("dd/MM/yyyy");
                    }

                    //Agrego la fila del requerimiento
                    dtInscripcion.Rows.Add(filaInscripcion);
                }

                //Seteo el datasource al reclamo
                objectDataSourceInscripcion.DataSource = dtInscripcion;
                reporte.DataSource = objectDataSourceInscripcion;

                result.Return = reporte;
            }
            catch (Exception e)
            {
                result.Error = "Error procesando la solicitud";
                return result;
            }
            return result;
        }

        public Resultado<string> GetInscripcionesPorChapa(_Model.Enums.TipoAuto? tipoAuto, int? numero)
        {
            var resultado = new Resultado<string>();

            if (tipoAuto == null || !numero.HasValue || numero.ToString().Count()>4)
            {
                resultado.Error = "Ingrese datos válidos";
                return resultado;
            }

            var resultadoInscripcion = _InscripcionRules.GetReporte(tipoAuto,numero);
            if (!resultadoInscripcion.Ok)
            {
                resultado.Error = resultadoInscripcion.Error;
                return resultado;
            }
            var inscripciones = resultadoInscripcion.Return;
            if (inscripciones.Count == 0)
            {
                resultado.Error = "No se encontraron inscripciones con los filtros ingresados";
            }

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Reporte_Inscripcion_Licencia_encabezado.trdx");
                //var rutaReportePadre = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.FullName;

                //var rutaReporte = rutaReportePadre + "/_Rules/Resources/Reportes/Reporte_Inscripcion_Licencia_encabezado.trdx";

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                System.Xml.XmlReader xmlReaderRequerimiento = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerRequerimiento = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerRequerimiento.Deserialize(xmlReaderRequerimiento);

                //---------------------------------
                // Defino el DT
                //---------------------------------

                Telerik.Reporting.ObjectDataSource objectDataSourceInscripcion = new Telerik.Reporting.ObjectDataSource();
                DataTable dtInscripcionLicencia = new DataTable();

                //Campos
                dtInscripcionLicencia.Columns.Add("Identificador");
                dtInscripcionLicencia.Columns.Add("tipoAuto");

                //-------------------------------------
                // Creo una fila y le cargo los datos
                //-------------------------------------

                DataRow filaLicencia = dtInscripcionLicencia.NewRow();

                //****************Encabezado*********************************
                //Num licencia
                string identificador = inscripciones[0].Identificador;
                if (string.IsNullOrEmpty(identificador))
                {
                    identificador = "Sin Datos";
                }
                filaLicencia["Identificador"] = identificador;

                //TipoAuto
                string tipoAutomovil = inscripciones[0].TipoAuto.Nombre;
                if (string.IsNullOrEmpty(identificador))
                {
                    tipoAutomovil = "Sin Datos";
                }
                filaLicencia["tipoAuto"] = tipoAutomovil;


                Telerik.Reporting.SubReport subReportItem = (Telerik.Reporting.SubReport)(reporte.Items.Find("subreporteInsripcionesLicencia", true)[0]);
                subReportItem.ReportSource = crearSubreporteInscripcionesLicencia(inscripciones).Return;

                //Agrego la fila del requerimiento
                dtInscripcionLicencia.Rows.Add(filaLicencia);

                //Seteo el datasource al reclamo
                objectDataSourceInscripcion.DataSource = dtInscripcionLicencia;
                reporte.DataSource = objectDataSourceInscripcion;

                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
                resultado.Return = base64;

            }
            catch (Exception e)
            {
                resultado.Error = e.Message;
                return resultado;
            }

            return resultado;
        }
        public Resultado<Telerik.Reporting.Report> crearSubreporteInscripcionesLicencia(List<Inscripcion> inscripciones)
        {
            var result = new Resultado<Telerik.Reporting.Report>();

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Reporte_Inscripcion_Licencia_detalle.trdx");
                //var rutaReportePadre = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.FullName;

                //var rutaReporte = rutaReportePadre + "/_Rules/Resources/Reportes/Reporte_Inscripcion_Licencia_detalle.trdx";

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                System.Xml.XmlReader xmlReaderRequerimiento = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerRequerimiento = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerRequerimiento.Deserialize(xmlReaderRequerimiento);

                //---------------------------------
                // Defino el DT
                //---------------------------------

                Telerik.Reporting.ObjectDataSource objectDataSourceInscripcion = new Telerik.Reporting.ObjectDataSource();
                DataTable dtInscripcion = new DataTable();

                //Campos

                dtInscripcion.Columns.Add("FechaInicio");
                dtInscripcion.Columns.Add("FechaFin");
                dtInscripcion.Columns.Add("FechaTelegrama");


                dtInscripcion.Columns.Add("Nombre");
                dtInscripcion.Columns.Add("Apellido");
                dtInscripcion.Columns.Add("Documento");
                dtInscripcion.Columns.Add("DomicilioCalle");
                dtInscripcion.Columns.Add("DomicilioAltura");
                dtInscripcion.Columns.Add("DomicilioBarrio");

                foreach (var i in inscripciones)
                {
                    //-------------------------------------
                    // Creo una fila y le cargo los datos
                    //-------------------------------------

                    DataRow filaInscripcion = dtInscripcion.NewRow();
                    if (i == null)
                    {
                        return null;
                    }


                    var fechaInicio = i.FechaInicio;
                    if (!string.IsNullOrEmpty(fechaInicio.ToString()))
                    {
                        filaInscripcion["FechaInicio"] = i.FechaInicio.Value.ToString("dd/MM/yyyy");
                    }

                    var fechaFin = i.FechaFin;
                    if (!string.IsNullOrEmpty(fechaFin.ToString()))
                    {
                        filaInscripcion["FechaFin"] = i.FechaFin.Value.ToString("dd/MM/yyyy");
                    }

                    var fechaTelegrama = i.FechaTelegrama;
                    if (!string.IsNullOrEmpty(fechaTelegrama.ToString()))
                    {
                        filaInscripcion["FechaTelegrama"] = fechaTelegrama.Value.ToString("dd/MM/yyyy");
                    }
                    //****************USUARIO********************************

                    string nombre = "";
                    string apellido = "";
                    string dni = "";
                    string calle = "";
                    var altura = "";
                    string barrio = "";
                    
                    if (i.Usuario == null)
                    {
                        nombre = "Sin Datos";
                        apellido = "Sin Datos";
                        dni = "Sin Datos";
                        calle = "Sin Datos";
                        altura = "Sin Datos";
                        barrio = "Sin Datos";
                    }
                    else
                    {
                        var usuario = i.Usuario;

                        //Nombre
                        nombre = usuario.Nombre;
                        if (string.IsNullOrEmpty(nombre))
                        {
                            nombre = "";
                        }
                        filaInscripcion["Nombre"] = nombre;
                        //Apellido
                        apellido = usuario.Apellido;
                        if (string.IsNullOrEmpty(apellido))
                        {
                            apellido = "";
                        }
                        filaInscripcion["Apellido"] = apellido;

                        //Dni
                        dni = usuario.Dni.ToString();
                        if (string.IsNullOrEmpty(dni))
                        {
                            dni = "Sin Datos";
                        }
                        filaInscripcion["Documento"] = dni;

                        //Calle
                        calle = usuario.DomicilioCalle;
                        if (string.IsNullOrEmpty(calle))
                        {
                            calle = "Sin Datos";
                        }
                        filaInscripcion["DomicilioCalle"] = calle;
                        //Altura
                        altura = usuario.DomicilioAltura;
                        if (altura == null)
                        {
                            altura = "Sin Datos";
                        }
                        filaInscripcion["DomicilioAltura"] = altura;
                        //Barrio
                        barrio = usuario.DomicilioBarrio;
                        if (string.IsNullOrEmpty(barrio))
                        {
                            barrio = "Sin Datos";
                        }
                        filaInscripcion["DomicilioBarrio"] = barrio;

                    }
                    //Agrego la fila del requerimiento
                    dtInscripcion.Rows.Add(filaInscripcion);
                }

                //Seteo el datasource al reclamo
                objectDataSourceInscripcion.DataSource = dtInscripcion;
                reporte.DataSource = objectDataSourceInscripcion;

                result.Return = reporte;
            }
            catch (Exception e)
            {
                result.Error = "Error procesando la solicitud";
                return result;
            }
            return result;
        }
    }
}
        
        //public Resultado<string> GetInscripcionesPorDni(_Model.Consultas.Consulta_Inscripcion consulta)
        //{
        //    var resultado = new Resultado<string>();
        //    var resultadoInscripcion = _InscripcionRules.Get(consulta);
        //    if (!resultadoInscripcion.Ok)
        //    {
        //        resultado.Error = resultadoInscripcion.Error;
        //        return resultado;
        //    }
        //    var inscripcion = resultadoInscripcion.Return;

        //    try
        //    {
        //        //var rutaReporte = HttpContext.Current.Server.MapPath("~/");
        //        var rutaReportePadre = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.FullName;

        //        var rutaReporte = rutaReportePadre + "/_Rules/Resources/Reportes/Reporte_Inscripcion_Dni.trdx";

        //        Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
        //        System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
        //        settings.IgnoreWhitespace = true;

        //        System.Xml.XmlReader xmlReaderRequerimiento = System.Xml.XmlReader.Create(rutaReporte, settings);
        //        Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerRequerimiento = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
        //        reporte = (Telerik.Reporting.Report)xmlSerializerRequerimiento.Deserialize(xmlReaderRequerimiento);

        //        //---------------------------------
        //        // Defino el DT
        //        //---------------------------------

        //        Telerik.Reporting.ObjectDataSource objectDataSourceInscripcion = new Telerik.Reporting.ObjectDataSource();
        //        DataTable dtInscripcion = new DataTable();

        //        //Campos
        //        dtInscripcion.Columns.Add("Nombre");
        //        dtInscripcion.Columns.Add("Apellido");
        //        dtInscripcion.Columns.Add("Documento");
        //        dtInscripcion.Columns.Add("DomicilioCalle");
        //        dtInscripcion.Columns.Add("DomicilioAltura");
        //        dtInscripcion.Columns.Add("DomicilioBarrio");

        //        dtInscripcion.Columns.Add("TipoAuto");
        //        dtInscripcion.Columns.Add("idTipoAuto");
        //        dtInscripcion.Columns.Add("identificador");


        //        //-------------------------------------
        //        // Creo una fila y le cargo los datos
        //        //-------------------------------------

        //        DataRow filaInscripcion = dtInscripcion.NewRow();

        //        //****************Encabezado*********************************
        //        //****************USUARIO********************************
        //        Usuario usuario = inscripcion[0].Usuario;
        //        //Nombre
        //        string nombre = usuario.Nombre;
        //        if (string.IsNullOrEmpty(nombre))
        //        {
        //            nombre = "Sin Datos";
        //        }
        //        filaInscripcion["Nombre"] = nombre;

        //        //Apellido
        //        string apellido = usuario.Apellido;
        //        if (string.IsNullOrEmpty(nombre))
        //        {
        //            apellido = "Sin Datos";
        //        }
        //        filaInscripcion["Apellido"] = apellido;

        //        //Dni
        //        string dni = usuario.Dni.ToString();
        //        if (string.IsNullOrEmpty(dni))
        //        {
        //            dni = "Sin Datos";
        //        }
        //        filaInscripcion["Documento"] = dni;

        //        //Calle
        //        string calle = usuario.DomicilioCalle;
        //        if (string.IsNullOrEmpty(calle))
        //        {
        //            calle = "Sin Datos";
        //        }
        //        filaInscripcion["DomicilioCalle"] = calle;
        //        //Altura
        //        string altura = usuario.DomicilioAltura.ToString();
        //        if (string.IsNullOrEmpty(altura))
        //        {
        //            altura = "Sin Datos";
        //        }
        //        filaInscripcion["DomicilioAltura"] = altura;
        //        //Barrio
        //        string barrio = usuario.DomicilioBarrio;
        //        if (string.IsNullOrEmpty(barrio))
        //        {
        //            barrio = "Sin Datos";
        //        }
        //        filaInscripcion["DomicilioBarrio"] = barrio;
        //        //***************TABLA FECHAS********************************

        //        DataTable dtFechas = new DataTable();
        //        dtFechas.Columns.Add("FechaInicio");
        //        dtFechas.Columns.Add("FechaFin");
        //        dtFechas.Columns.Add("FechaTelegrama");


        //        foreach (Inscripcion i in inscripcion)
        //        {
        //            DataRow filaFechas = dtFechas.NewRow();

        //            filaInscripcion["TipoAuto"] = i.TipoAuto.Nombre;
        //            filaInscripcion["idTipoAuto"] = i.TipoAuto.Id;
        //            filaInscripcion["identificador"] = i.Identificador;



        //            var fechaInicio = i.FechaInicio;
        //            if (!string.IsNullOrEmpty(fechaInicio.ToString()))
        //            {
        //                filaFechas["FechaInicio"] = i.FechaInicio.Value.ToString("dd/MM/yyyy");
        //            }

        //            var fechaFin = i.FechaFin;
        //            if (!string.IsNullOrEmpty(fechaFin.ToString()))
        //            {
        //                filaFechas["FechaFin"] = i.FechaFin.Value.ToString("dd/MM/yyyy");
        //            }

        //            var fechaTelegrama = i.FechaTelegrama;
        //            if (!string.IsNullOrEmpty(fechaTelegrama.ToString()))
        //            {
        //                filaFechas["FechaTelegrama"] = fechaTelegrama.Value.ToString("dd/MM/yyyy");
        //            }


        //            dtFechas.Rows.Add(filaFechas);
        //            //dtDescripcion.Rows.Add(filaDescripcion);
        //        }
        //        Telerik.Reporting.Table tFechas = (Telerik.Reporting.Table)(reporte.Items.Find("tablaFechas", true)[0]);
        //        tFechas.DataSource = dtFechas;

        //        //Agrego la fila del requerimiento
        //        dtInscripcion.Rows.Add(filaInscripcion);

        //        //Seteo el datasource al reclamo
        //        objectDataSourceInscripcion.DataSource = dtInscripcion;
        //        reporte.DataSource = objectDataSourceInscripcion;

        //        Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
        //        RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
        //        var bytes = result.DocumentBytes;
        //        var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
        //        resultado.Return = base64;

        //    }
        //    catch (Exception e)
        //    {
        //        resultado.Error = "Error procesando la solicitud";
        //        return resultado;
        //    }

        //    return resultado;
        //}

    
       
