using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_TipoCondicionInscripcion
    {
        public int KeyValue { get; set; }
        public string Nombre { get; set; }

        public ResultadoWS_TipoCondicionInscripcion(TipoCondicionInscripcion entity)
        {
            if (entity == null) return;

            Nombre = entity.Nombre;
            KeyValue = (int)entity.KeyValue;
        }

        public static List<ResultadoWS_TipoCondicionInscripcion> ToList(IList<TipoCondicionInscripcion> list)
        {
            return list.Select(x => new ResultadoWS_TipoCondicionInscripcion(x)).ToList();

        }
    }
}