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
        public string Nombre { get; set; }
        public _Model.Enums.TipoInhabilitacion? TipoInhabilitacion { get; set; }
        public bool? DadosDeBaja { get; set; }
        public bool? ConError{ get; set; }
        public bool? Favorito{ get; set; }

        public Consulta_Inhabilitacion()
        {
            DadosDeBaja = false;
        }
    }
}
