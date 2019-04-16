using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Comandos
{
    public class Comando_InhabilitacionNuevo
    {
        public int? IdUsuario { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public virtual string DtoRes { get; set; }
        public virtual string Expediente { get; set; }
        public virtual string Observaciones { get; set; }
        public string ObservacionesAutoChapa { get; set; }
        public string ObservacionesTipoAuto { get; set; }
        public Enums.TipoInhabilitacion? TipoInhabilitacionKeyValue { get; set; }
    }
}
