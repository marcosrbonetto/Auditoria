using System;
using System.Linq;
using _Model.Entities;
using _Model;

namespace _DAO.Maps
{
    class InhabilitacionMap : BaseEntityMap<Inhabilitacion>
    {
        public InhabilitacionMap()
        {
            Table("Inhabilitacion");

            References<Usuario>(x => x.Usuario, "idUsuario");
            Map(x => x.UsuarioDni, "usuarioDni");

            References<TipoInhabilitacion>(x => x.TipoInhabilitacion, "idTipoInhabilitacion");

            Map(x => x.FechaInicio, "fechaInicio");
            Map(x => x.FechaInicioString, "fechaInicioString");

            Map(x => x.FechaFin, "fechaFin");
            Map(x => x.FechaFinString, "fechaFinString");

            Map(x => x.DtoRes, "dtoRes");
            Map(x => x.Expediente, "expediente");

            Map(x => x.Observaciones, "observaciones");
            Map(x => x.ObservacionesAutoChapa, "observacionesAutoChapa");
            Map(x => x.ObservacionesTipoAuto, "observacionesTipoAuto");
            Map(x => x.Error, "error");
            Map(x => x.Favorito, "favorito");

        }
    }
}