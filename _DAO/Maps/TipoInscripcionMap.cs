using System;
using System.Linq;
using _Model.Entities;
using _Model;

namespace _DAO.Maps
{
    class TipoInscripcionMap : BaseEntityMap<TipoInscripcion>
    {
        public TipoInscripcionMap()
        {
            Table("TipoInscripcion");

            Map(x => x.Nombre, "nombre");
            Map(x => x.KeyValue, "keyValue").CustomType(typeof(Enums.TipoInscripcion));
        }
    }
}