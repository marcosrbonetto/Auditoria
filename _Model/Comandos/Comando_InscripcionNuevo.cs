using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Comandos
{
    public class Comando_InscripcionNuevo
    {
        public int? IdUsuario { get; set; }
        public Enums.TipoAuto? TipoAutoKeyValue { get; set; }
        public string Identificador { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public Enums.TipoInscripcion? TipoInscripcionKeyValue { get; set; }
        public string FechaTelegrama { get; set; }
        public string FechaVencimientoLicencia { get; set; }
        public string ArtCompañia { get; set; }
        public string ArtFechaVencimiento { get; set; }
        public string Caja { get; set; }
        public string Observaciones { get; set; }
        public Enums.TipoCondicionInscripcion? TipoCondicionInscripcionKeyValue { get; set; }
    }

}
