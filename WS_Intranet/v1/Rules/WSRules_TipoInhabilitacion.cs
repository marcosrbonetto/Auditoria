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
    public class WSRules_TipoInhabilitacion : WSRules_Base
    {
        private readonly Rules_TipoInhabilitacion rules;

        public WSRules_TipoInhabilitacion(UsuarioLogueado data)
            : base(data)
        {
            rules = new Rules_TipoInhabilitacion(data);
        }


        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoInhabilitacion>> Get()
        {
            var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoInhabilitacion>>();

            var resultadoQuery = rules.GetAll(false);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            resultado.Return = v1.Entities.Resultados.ResultadoWS_TipoInhabilitacion.ToList(resultadoQuery.Return);
            return resultado;
        }
    }
}