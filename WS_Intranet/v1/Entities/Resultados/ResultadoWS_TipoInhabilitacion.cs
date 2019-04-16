using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_TipoInhabilitacion
    {
        public int KeyValue { get; set; }
        public string Nombre { get; set; }

        public ResultadoWS_TipoInhabilitacion(TipoInhabilitacion entity)
        {
            if (entity == null) return;

            Nombre = entity.Nombre;
            KeyValue = (int)entity.KeyValue;
        }

        public static List<ResultadoWS_TipoInhabilitacion> ToList(IList<TipoInhabilitacion> list)
        {
            return list.Select(x => new ResultadoWS_TipoInhabilitacion(x)).ToList();

        }
    }
}