using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Consultas
{
    public class Consulta_MuniOnlineUsuario
    {
        public int? Dni { get; set; }
        public bool? SexoMasculino { get; set; }
        public bool? DadosDeBaja { get; set; }
        public bool? ValidadoRenaper { get; set; }

        public Consulta_MuniOnlineUsuario()
        {
            DadosDeBaja = false;
        }
    }
}
