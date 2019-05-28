using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Consultas
{
    public class Consulta_Inscripcion
    {
        public int? Dni { get; set; }
        public string Nombre { get; set; }
        public bool? Sexo { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime? ActivoHasta { get; set; }
        public string Identificador { get; set; }
        public Enums.TipoAuto? TipoAuto { get; set; }
        public Enums.TipoInscripcion? TipoInscripcion { get; set; }
        public bool? DadosDeBaja { get; set; }
        public bool? ConFechaInicio { get; set; }
        public bool? ConError{ get; set; }
        public bool? Favorito { get; set; }

        public Consulta_Inscripcion()
        {
            DadosDeBaja = false;
        }
    }
}
