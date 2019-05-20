using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Intranet.v0;
using WS_Intranet.v0.Controllers;
using WS_Intranet.v0.Controllers.FilterAtributtes;
using WS_Intranet.v1.Controllers.FilterAtributtes;
using WS_Intranet.v1.Entities.Consultas;
using WS_Intranet.v1.Entities.Resultados;
using WS_Intranet.v1.Rules;

namespace WS_Intranet.v1.Controllers
{
    [RoutePrefix("v1/Reporte")]
    public class Reporte_v1Controller : _Control
    {

        [HttpPost]
        [ConToken]
        [EsOperador]
        [Route("GetInscripcionesPorDni")]
        public ResultadoServicio<string> GetInscripcionesPorDni(Consulta_Inscripcion consulta)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<string>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_Reporte(usuarioLogeado.Return).GetInscripcionesPorDni(consulta);
        }

        [HttpPost]
        [ConToken]
        [EsOperador]
        [Route("GetInscripcionesPorChapa")]
        public ResultadoServicio<string> GetInscripcionesPorChapa(Consulta_Inscripcion consulta)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<string>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_Reporte(usuarioLogeado.Return).GetInscripcionesPorChapa(consulta);
        }
    }
}