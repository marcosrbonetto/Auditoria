using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Consultas
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

        public _Model.Consultas.Consulta_UsuarioPaginado Convertir()
        {
            return new _Model.Consultas.Consulta_UsuarioPaginado()
            {
                //Filtros
                Nombre = Nombre,
                Dni = Dni,
                SexoMasculino = SexoMasculino,
                FechaNacimiento = _Model.Utils.StringToDate(FechaNacimiento),
                DadosDeBaja = false,

                //Paginado
                Pagina = Pagina,
                TamañoPagina = TamañoPagina,
                OrderBy = (_Model.Enums.UsuarioOrderBy)OrderBy,
                OrderByAsc = OrderByAsc
            };
        }
    }
}