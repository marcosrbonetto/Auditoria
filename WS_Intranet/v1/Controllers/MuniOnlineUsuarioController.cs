using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Intranet.v0;
using WS_Intranet.v0.Controllers;
using WS_Intranet.v0.Controllers.FilterAtributtes;
using WS_Intranet.v1.Entities.Resultados;
using WS_Intranet.v1.Rules;

namespace WS_Intranet.v1.Controllers
{
    [RoutePrefix("v1/MuniOnlineUsuario")]
    public class MuniOnlineUsuario_v1Controller : _Control
    {

        [HttpPut]
        [Route("IniciarSesion")]
        public ResultadoServicio<string> IniciarSesion(v1.Entities.Comandos.ComandoWS_IniciarSesion comando)
        {
            return new v1.Rules.WSRules_MuniOnlineUsuario(null).IniciarSesion(comando);
        }

        [HttpPut]
        [ConToken]
        [Route("CerrarSesion")]
        public ResultadoServicio<bool> CerrarSesion()
        {
            var resultado = new ResultadoServicio<bool>();

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_MuniOnlineUsuario(usuarioLogeado.Return).CerrarSesion(usuarioLogeado.Return.Token);
        }

        [HttpGet]
        [ConToken]
        [Route("")]
        public ResultadoServicio<ResultadoWS_MuniOnlineUsuario> GetUsuario()
        {
            var resultado = new ResultadoServicio<ResultadoWS_MuniOnlineUsuario>();

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_MuniOnlineUsuario(usuarioLogeado.Return).GetUsuario(usuarioLogeado.Return.Token);
        }

        [HttpGet]
        [ConToken]
        [Route("ValidarToken")]
        public ResultadoServicio<bool> ValidarToken()
        {
            return base.ValidarToken();
        }

        [HttpGet]
        [ConToken]
        [Route("ValidadoRenaper")]
        public ResultadoServicio<bool> ValidadoRenaper()
        {
            var resultado = new ResultadoServicio<bool>();

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_MuniOnlineUsuario(usuarioLogeado.Return).ValidadoRenaper(usuarioLogeado.Return.Token);
        }

        [HttpGet]
        [ConToken]
        [Route("EsOperador")]
        public ResultadoServicio<bool> EsOperador()
        {
            var resultado = new ResultadoServicio<bool>();

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_MuniOnlineUsuario(usuarioLogeado.Return).EsOperador(usuarioLogeado.Return.IdUsuario);
        }
       
    }
}