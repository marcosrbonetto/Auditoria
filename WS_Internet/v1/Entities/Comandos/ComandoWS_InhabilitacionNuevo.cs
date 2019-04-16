using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Comandos
{
    public class ComandoWS_InhabilitacionNuevo
    {
        public int? IdUsuario { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string DtoRes { get; set; }
        public string Expediente { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesAutoChapa { get; set; }
        public string ObservacionesTipoAuto { get; set; }
        public int? TipoInhabilitacionKeyValue { get; set; }
    }
}