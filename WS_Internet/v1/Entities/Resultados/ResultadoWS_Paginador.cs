using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Internet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_Paginador<T>
    {
        public int PaginaActual { get; set; }
        public int TamañoPagina { get; set; }
        public int CantidadPaginas { get; set; }
        public int Count { get; set; }
        public int OrderBy { get; set; }
        public List<T> Data { get; set; }
    }
}