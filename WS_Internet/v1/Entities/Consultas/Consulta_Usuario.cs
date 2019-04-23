using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Consultas
{
    public class Consulta_Usuario
    {
        public int? Dni { get; set; }
        public bool? SexoMasculino { get; set; }
        public string Nombre { get; set; }
        public bool? ConError { get; set; }
        public bool? Favorito { get; set; }
    }
}