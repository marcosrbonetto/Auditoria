using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Consultas
{
    public class Consulta_Usuario
    {
        public string Nombre { get; set; }
        public int? Dni { get; set; }
        public bool? SexoMasculino { get; set; }
    }
}