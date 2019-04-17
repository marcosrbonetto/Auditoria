﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Consultas
{
    [Serializable]
    public class Consulta_InhabilitacionPaginada
    {
        public int? Dni { get; set; }
        public string Nombre { get; set; }

        //Paginada
        public int Pagina { get; set; }
        public int TamañoPagina { get; set; }
        public int OrderBy { get; set; }
        public bool OrderByAsc { get; set; }
    }
}