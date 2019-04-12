using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Consultas
{
    public class Consulta_UsuarioPaginado
    {
        public string Nombre { get; set; }
        public int? Dni { get; set; }
        public bool? SexoMasculino { get; set; }
        public string FechaNacimiento { get; set; }

        //Paginada
        public int Pagina { get; set; }
        public int TamañoPagina { get; set; }
        public int OrderBy { get; set; }
        public bool OrderByAsc { get; set; }
    }
}