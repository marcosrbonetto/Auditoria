using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Internet.v0;
using WS_Internet.v0.FilterAtributtes;

namespace WS_Internet.v1.Controllers
{
    [RoutePrefix("v1/MuniOnlineUsuario")]
    public class MuniOnlineUsuario_v1Controller : ApiController
    {
        [HttpPut]
        [Route("IniciarSesion")]
        public ResultadoServicio<string> IniciarSesion(v1.Entities.Comandos.ComandoWS_IniciarSesion comando)
        {
            return RestCall.Call<string>(Request, comando);
        }

        [HttpPut]
        [ConToken]
        [Route("CerrarSesion")]
        public ResultadoServicio<bool> CerrarSesion()
        {
            return RestCall.Call<bool>(Request);
        }

        [HttpGet]
        [ConToken]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_MuniOnlineUsuario> GetUsuario()
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_MuniOnlineUsuario>(Request);
        }

        [HttpGet]
        [ConToken]
        [Route("ValidarToken")]
        public ResultadoServicio<bool> ValidarToken()
        {
            return RestCall.Call<bool>(Request);
        }

        [HttpGet]
        [ConToken]
        [Route("ValidadoRenaper")]
        public ResultadoServicio<bool> ValidadoRenaper()
        {
            return RestCall.Call<bool>(Request);
        }

        [HttpGet]
        [ConToken]
        [Route("EsOperador")]
        public ResultadoServicio<bool> EsOperador()
        {
            return RestCall.Call<bool>(Request);
        }
    }
}