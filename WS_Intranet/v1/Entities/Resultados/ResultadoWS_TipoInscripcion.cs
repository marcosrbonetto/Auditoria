using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_TipoInscripcion
    {
        public int KeyValue { get; set; }
        public string Nombre { get; set; }

        public ResultadoWS_TipoInscripcion(TipoInscripcion entity)
        {
            if (entity == null) return;

            Nombre = entity.Nombre;
            KeyValue = (int)entity.KeyValue;
        }

        public static List<ResultadoWS_TipoInscripcion> ToList(IList<TipoInscripcion> list)
        {
            return list.Select(x => new ResultadoWS_TipoInscripcion(x)).ToList();

        }
    }
}