using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Comandos
{
    public class ComandoWS_InhabilitacionNuevo
    {
        public int IdUsuario { get; set; }
        public string Identificador { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int TipoAutoKeyValue { get; set; }
        public virtual string DtoRes { get; set; }
        public virtual string Expediente { get; set; }
        public virtual string FechaExpediente { get; set; }
        public virtual string Causa { get; set; }
        public virtual string Observaciones { get; set; }
    }
}