using System;
using System.Linq;
using _Model.Entities;
using _Model;

namespace _DAO.Maps
{
    class TipoInhabilitacionMap : BaseEntityMap<TipoInhabilitacion>
    {
        public TipoInhabilitacionMap()
        {
            Table("TipoInhabilitacion");

            Map(x => x.Nombre, "nombre");
            Map(x => x.KeyValue, "keyValue").CustomType(typeof(Enums.TipoInhabilitacion));
            Map(x => x.Permanente, "permanente");
            Map(x => x.Invalido, "invalido");

        }
    }
}