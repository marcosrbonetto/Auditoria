using System;
using System.Linq;
using _DAO.DAO;
using _Model;
using _Model.Entities;
using System.Configuration;
using System.Collections.Generic;
using _Rules._WsVecinoVirtual;
using System.Web;
using System.Data;

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

        }      
        public Resultado<TipoAuto> GetByKeyValue(Enums.TipoAuto keyValue)
        {
            return dao.GetByKeyValue(keyValue);
        }

        public Resultado<string> GetInscripcionesPorDni(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var resultado = new Resultado<string>();

            return resultado;
        }

        public Resultado<string> GetInscripcionesPorChapa(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var resultado = new Resultado<string>();

            return resultado;
        }

        public Resultado<Telerik.Reporting.Report> GenerarReporte(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var inscripcion =_InscripcionRules.Get(consulta);
            var resultado = new Resultado<Telerik.Reporting.Report>();

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/_Rules/Resources/Reportes/Reporte_Inscripcion_Dni.trdx");

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

                //Lado izq del reporte
                dtInscripcion.Columns.Add("Nombre");
                
            

                //-------------------------------------
                // Creo una fila y le cargo los datos
                //-------------------------------------

                DataRow filaInscripcion = dtInscripcion.NewRow();

                //****************Encabezado*********************************

                //Numero
                string nombre = inscripcion.Return[0].Usuario.Nombre;
                if (string.IsNullOrEmpty(nombre))
                {
                    nombre = "Sin Datos";
                }
                filaInscripcion["Numero"] = nombre;
                
                //Agrego la fila del requerimiento
                dtInscripcion.Rows.Add(filaInscripcion);

                //Seteo el datasource al reclamo
                objectDataSourceInscripcion.DataSource = dtInscripcion;
                reporte.DataSource = objectDataSourceInscripcion;

                resultado.Return = reporte;
            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            return resultado;
        }

    }
}