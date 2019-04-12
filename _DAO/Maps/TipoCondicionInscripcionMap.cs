using System;
using System.Linq;
using _Model.Entities;
using _Model;

namespace _DAO.Maps
{
    class TipoCondicionInscripcionMap : BaseEntityMap<TipoCondicionInscripcion>
    {
        public TipoCondicionInscripcionMap()
        {
            Table("CondicionInscripcion");

            Map(x => x.Nombre, "nombre");
            Map(x => x.KeyValue, "keyValue").CustomType(typeof(Enums.TipoCondicionInscripcion));
        }
    }
}