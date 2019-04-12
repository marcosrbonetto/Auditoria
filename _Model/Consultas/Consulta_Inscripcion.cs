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
        public string Identificador { get; set; }
        public bool? DadosDeBaja { get; set; }
        public bool? ConFechaInicio { get; set; }

        public Consulta_Inscripcion()
        {
            DadosDeBaja = false;
        }
    }
}
