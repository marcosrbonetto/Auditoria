using System;
using System.Linq;
using WS_Intranet.v1.Entities.Resultados;
using _Rules;
using WS_Intranet.v0.Rules;
using WS_Intranet.v0;
using _Rules.Rules;
using System.Collections.Generic;

namespace WS_Intranet.v1.Rules
{
    public class WSRules_TipoInscripcion : WSRules_Base
    {
        private readonly Rules_TipoInscripcion rules;

        public WSRules_TipoInscripcion(UsuarioLogueado data)
            : base(data)
        {
            rules = new Rules_TipoInscripcion(data);
        }


        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoInscripcion>> Get()
        {
            var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoInscripcion>>();

            var resultadoQuery = rules.GetAll(false);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            resultado.Return = v1.Entities.Resultados.ResultadoWS_TipoInscripcion.ToList(resultadoQuery.Return);
            return resultado;
        }
    }
}