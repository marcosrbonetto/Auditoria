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
        }
    }
}