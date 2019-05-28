using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Consultas
{
    [Serializable]
    public class Consulta_Inscripcion
    {
        public int? Dni { get; set; }
        public string Identificador { get; set; }
        public string Nombre { get; set; }
        public bool? Sexo { get; set; }
        public _Model.Enums.TipoInscripcion? TipoInscripcion { get; set; }
        public _Model.Enums.TipoAuto? TipoAuto { get; set; }
        public bool? ConFechaInicio { get; set; }
        public bool? ConError { get; set; }
        public bool? Favorito{ get; set; }

        public _Model.Consultas.Consulta_Inscripcion Convertir()
        {
            return new _Model.Consultas.Consulta_Inscripcion()
            {
                Dni = Dni,
                Nombre = Nombre,
                Identificador = Identificador,
                ConFechaInicio = ConFechaInicio,
                TipoInscripcion = TipoInscripcion,
                TipoAuto = TipoAuto,
                ConError = ConError,
                Favorito = Favorito,
                Sexo = Sexo,
                DadosDeBaja = false
            };
        }
    }
}