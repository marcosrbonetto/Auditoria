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
    public class WSRules_TipoCondicionInscripcion : WSRules_Base
    {
        private readonly Rules_TipoCondicionInscripcion rules;

        public WSRules_TipoCondicionInscripcion(UsuarioLogueado data)
            : base(data)
        {
            rules = new Rules_TipoCondicionInscripcion(data);
        }


        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoCondicionInscripcion>> Get()
        {
            var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoCondicionInscripcion>>();

            var resultadoQuery = rules.GetAll(false);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            resultado.Return = v1.Entities.Resultados.ResultadoWS_TipoCondicionInscripcion.ToList(resultadoQuery.Return);
            return resultado;
        }
    }
}