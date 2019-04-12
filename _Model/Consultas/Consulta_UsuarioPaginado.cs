using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Consultas
{
    public class Consulta_UsuarioPaginado: Consulta_Usuario
    {
        public Enums.UsuarioOrderBy OrderBy { get; set; }
        public bool OrderByAsc { get; set; }
        public int Pagina { get; set; }
        public int TamañoPagina { get; set; }
    }
}
