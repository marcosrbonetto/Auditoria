using System;
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
    [RoutePrefix("v1/Inhabilitacion")]
    public class Inhabilitacion_v1Controller : _Control
    {
        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("BuscarPaginado")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InhabilitacionListado>> BuscarPaginado(v1.Entities.Consultas.Consulta_InhabilitacionPaginada consulta)
        {

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InhabilitacionListado>>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inhabilitacion(usuarioLogeado.Return).BuscarPaginado(consulta);
        }

        [HttpGet]
        [ConToken]
        [EsOperador]
        [Route("Detalle")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> GetDetalle(int id)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inhabilitacion(usuarioLogeado.Return).GetDetalle(id);
        }

        [HttpPost]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> Insertar(v1.Entities.Comandos.ComandoWS_InhabilitacionNuevo comando)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inhabilitacion(usuarioLogeado.Return).Insertar(comando);
        }

        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> Actualizar(v1.Entities.Comandos.ComandoWS_InhabilitacionActualizar comando)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inhabilitacion(usuarioLogeado.Return).Actualizar(comando);
        }


        [HttpDelete]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<bool> Borrar(int id)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<bool>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inhabilitacion(usuarioLogeado.Return).Borrar(id);
        }
    }
}