using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Comandos
{
    public class ComandoWS_InscripcionNuevo
    {
        public int? IdUsuario { get; set; }
        public string Identificador { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int? TipoAutoKeyValue { get; set; }
        public int? TipoInscripcionKeyValue { get; set; }
        public string FechaTelegrama { get; set; }
        public string FechaVencimientoLicencia { get; set; }
        public string ArtCompañia { get; set; }
        public string ArtFechaVencimiento { get; set; }
        public string Caja { get; set; }
        public string Observaciones { get; set; }
        public int? TipoCondicionInscripcionKeyValue { get; set; }
    }
}