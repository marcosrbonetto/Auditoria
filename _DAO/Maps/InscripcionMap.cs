using System;
using System.Linq;
using _Model.Entities;
using _Model;

namespace _DAO.Maps
{
    class InscripcionMap : BaseEntityMap<Inscripcion>
    {
        public InscripcionMap()
        {
            Table("Inscripcion");

            References<Usuario>(x => x.Usuario, "idUsuario");
            Map(x => x.UsuarioDni, "usuarioDni");
            
            References<TipoInscripcion>(x => x.TipoInscripcion, "idTipoInscripcion");

            References<TipoAuto>(x => x.TipoAuto, "idTipoAuto");

            Map(x => x.Identificador, "identificador");
            Map(x => x.FechaInicio, "fechaInicio");
            Map(x => x.FechaFin, "fechaFin");
            Map(x => x.FechaTelegrama, "fechaTelegrama");
            Map(x => x.FechaVencimientoLicencia, "fechaVencimientoLicencia");
            Map(x => x.ArtCompañia, "artCompañia");
            Map(x => x.ArtFechaVencimiento, "artFechaVencimiento");
            Map(x => x.Caja, "caja");
            Map(x => x.Observaciones, "observaciones");
            Map(x => x.Error, "error");

        }
    }
}