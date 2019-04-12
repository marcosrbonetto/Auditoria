using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Consultas
{
    public class Consulta_Inhabilitacion
    {
        public int? Dni { get; set; }
        public bool? DadosDeBaja { get; set; }

        public Consulta_Inhabilitacion()
        {
            DadosDeBaja = false;
        }
    }
}
