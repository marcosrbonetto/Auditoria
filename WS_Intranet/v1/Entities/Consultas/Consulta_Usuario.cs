using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Consultas
{
    public class Consulta_Usuario
    {
        public string Nombre { get; set; }
        public int? Dni { get; set; }
        public bool? SexoMasculino { get; set; }

        public _Model.Consultas.Consulta_Usuario Convertir()
        {
            return new _Model.Consultas.Consulta_Usuario()
            {
                Nombre = Nombre,
                Dni = Dni,
                SexoMasculino = SexoMasculino,
                DadosDeBaja = false
            };
        }
    }
}