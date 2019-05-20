using System;
using System.Linq;
using WS_Intranet.v1.Entities.Resultados;
using _Rules;
using WS_Intranet.v0.Rules;
using WS_Intranet.v0;
using _Rules.Rules;
using System.Collections.Generic;
using WS_Intranet.v1.Entities.Consultas;

namespace WS_Intranet.v1.Rules
{
    public class WSRules_Reporte : WSRules_Base
    {
        private readonly Rules_Reportes rules;

        public WSRules_Reporte(UsuarioLogueado data)
            : base(data)
        {
            rules = new Rules_Reportes(data);
        }


        public ResultadoServicio<string> GetInscripcionesPorDni(Consulta_Inscripcion consulta)
        {
            var resultado = new ResultadoServicio<string>();

            var resultadoQuery = rules.GetInscripcionesPorDni(consulta);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            resultado.Return = resultadoQuery.Return;
            return resultado;
        }

        public ResultadoServicio<string> GetInscripcionesPorChapa(Consulta_Inscripcion consulta)
        {
            var resultado = new ResultadoServicio<string>();

            var resultadoQuery = rules.GetInscripcionesPorChapa(consulta);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            resultado.Return = resultadoQuery.Return;
            return resultado;
        }
    }
}