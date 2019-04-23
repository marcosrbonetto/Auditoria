using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Consultas
{
    [Serializable]
    public class Consulta_InscripcionPaginada
    {
        public int? Dni { get; set; }
        public string Nombre { get; set; }
        public string Identificador { get; set; }
        public int? TipoInscripcion { get; set; }
        public int? TipoAuto { get; set; }
        public bool? ConFechaInicio { get; set; }
        public bool? ConError { get; set; }
        public bool? Favorito { get; set; }

        //Paginada
        public int Pagina { get; set; }
        public int TamañoPagina { get; set; }
        public int OrderBy { get; set; }
        public bool OrderByAsc { get; set; }
    }
}