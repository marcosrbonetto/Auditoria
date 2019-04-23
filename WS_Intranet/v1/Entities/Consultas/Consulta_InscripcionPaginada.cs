using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Consultas
{
    [Serializable]
    public class Consulta_InscripcionPaginada
    {
        public int? Dni { get; set; }
        public string Identificador { get; set; }
        public string Nombre { get; set; }
        public _Model.Enums.TipoInscripcion? TipoInscripcion { get; set; }
        public _Model.Enums.TipoAuto? TipoAuto { get; set; }
        public bool? ConFechaInicio { get; set; }
        public bool? ConError { get; set; }
        public bool? Favorito{ get; set; }

        //Paginada
        public int Pagina { get; set; }
        public int TamañoPagina { get; set; }
        public int OrderBy { get; set; }
        public bool OrderByAsc { get; set; }


        public _Model.Consultas.Consulta_InscripcionPaginada Convertir()
        {
            return new _Model.Consultas.Consulta_InscripcionPaginada()
            {
                Dni = Dni,
                Nombre = Nombre,
                Identificador = Identificador,
                ConFechaInicio = ConFechaInicio,
                TipoInscripcion = TipoInscripcion,
                TipoAuto = TipoAuto,
                ConError = ConError,
                Favorito = Favorito,
                Pagina = Pagina,
                TamañoPagina = TamañoPagina,
                OrderBy = (_Model.Enums.InscripcionOrderBy)OrderBy,
                OrderByAsc = OrderByAsc,
                DadosDeBaja = false
            };
        }
    }
}