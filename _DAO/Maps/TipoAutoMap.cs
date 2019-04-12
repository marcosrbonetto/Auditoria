using System;
using System.Linq;
using _Model.Entities;
using _Model;

namespace _DAO.Maps
{
    class TipoAutoMap : BaseEntityMap<TipoAuto>
    {
        public TipoAutoMap()
        {
            Table("TipoAuto");

            Map(x => x.Nombre, "nombre");
            Map(x => x.KeyValue, "keyValue").CustomType(typeof(Enums.TipoAuto));
        }
    }
}