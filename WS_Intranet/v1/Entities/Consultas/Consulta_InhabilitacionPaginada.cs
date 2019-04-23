using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Consultas
{
    [Serializable]
    public class Consulta_InhabilitacionPaginada
    {
        public _Model.Enums.TipoInhabilitacion? TipoInhabilitacion { get; set; }
        public int? Dni { get; set; }
        public string Nombre { get; set; }
        public bool? ConError { get; set; }
        public bool? Favorito { get; set; }

        //Paginada
        public int Pagina { get; set; }
        public int TamañoPagina { get; set; }
        public int OrderBy { get; set; }
        public bool OrderByAsc { get; set; }


        public _Model.Consultas.Consulta_InhabilitacionPaginada Convertir()
        {
            return new _Model.Consultas.Consulta_InhabilitacionPaginada()
            {
                TipoInhabilitacion=TipoInhabilitacion,
                Dni = Dni,
                Nombre = Nombre,
                ConError = ConError,
                Favorito = Favorito,
                Pagina = Pagina,
                TamañoPagina = TamañoPagina,
                OrderBy = (_Model.Enums.InhabilitacionOrderBy)OrderBy,
                OrderByAsc = OrderByAsc,
                DadosDeBaja = false
            };
        }
    }
}