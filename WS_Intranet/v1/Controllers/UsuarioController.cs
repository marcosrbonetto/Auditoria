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
    [RoutePrefix("v1/Usuario")]
    public class Usuario_v1Controller : _Control
    {

        [HttpPost]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> Insertar(v1.Entities.Comandos.ComandoWS_UsuarioNuevo comando)
        {

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_Usuario(usuarioLogeado.Return).Insertar(comando);
        }

        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> Actualizar(v1.Entities.Comandos.ComandoWS_UsuarioActualizar comando)
        {

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_Usuario(usuarioLogeado.Return).Actualizar(comando);
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

            return new v1.Rules.WSRules_Usuario(usuarioLogeado.Return).Borrar(id);
        }


        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("Buscar")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_Usuario>> Buscar(v1.Entities.Consultas.Consulta_Usuario consulta)
        {

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_Usuario>>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_Usuario(usuarioLogeado.Return).Buscar(consulta);
        }

        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("BuscarPaginado")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_Usuario>> BuscarPaginado(v1.Entities.Consultas.Consulta_UsuarioPaginado consulta)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_Usuario>>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_Usuario(usuarioLogeado.Return).BuscarPaginado(consulta);
        }


        [HttpGet]
        [ConToken]
        [EsOperador]
        [Route("Detalle")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> GetDetalle(int id)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_Usuario(usuarioLogeado.Return).GetDetalle(id);
        }
    }
}