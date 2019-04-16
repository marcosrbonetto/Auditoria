﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Intranet.v0;
using WS_Intranet.v0.Controllers;
using WS_Intranet.v0.Controllers.FilterAtributtes;
using WS_Intranet.v1.Controllers.FilterAtributtes;
using WS_Intranet.v1.Entities.Resultados;
using WS_Intranet.v1.Rules;

namespace WS_Intranet.v1.Controllers
{
    [RoutePrefix("v1/TipoInhabilitacion")]
    public class TipoInhabilitacion_v1Controller : _Control
    {

        [HttpGet]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoInhabilitacion>> Get()
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoInhabilitacion>>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_TipoInhabilitacion(usuarioLogeado.Return).Get();
        }
    }
}