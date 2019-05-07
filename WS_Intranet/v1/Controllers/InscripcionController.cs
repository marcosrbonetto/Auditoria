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
    [RoutePrefix("v1/Inscripcion")]
    public class Inscripcion_v1Controller : _Control
    {
      
        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("BuscarPaginado")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InscripcionListado>> BuscarPaginado(v1.Entities.Consultas.Consulta_InscripcionPaginada consulta)
        {

            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InscripcionListado>>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inscripcion(usuarioLogeado.Return).BuscarPaginado(consulta);
        }

        [HttpGet]
        [ConToken]
        [EsOperador]
        [Route("Detalle")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> GetDetalle(int id)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inscripcion(usuarioLogeado.Return).GetDetalle(id);
        }

        [HttpPost]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> Insertar(v1.Entities.Comandos.ComandoWS_InscripcionNuevo comando)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inscripcion(usuarioLogeado.Return).Insertar(comando);
        }

        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> Actualizar(v1.Entities.Comandos.ComandoWS_InscripcionActualizar comando)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inscripcion(usuarioLogeado.Return).Actualizar(comando);
        }


        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("Favorito")]
        public ResultadoServicio<bool> ToggleFavorito(int id)
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<bool>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new WSRules_Inscripcion(usuarioLogeado.Return).ToggleFavorito(id);
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

            return new WSRules_Inscripcion(usuarioLogeado.Return).Borrar(id);
        }

        [HttpGet]
        [ConToken]
        [EsOperador]
        [Route("CantidadConError")]
        public ResultadoServicio<int> GetCantidadConError()
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<int>();
                resultado.Error = usuarioLogeado.Error;
                return resultado;
            }

            return new v1.Rules.WSRules_Inscripcion(usuarioLogeado.Return).GetCantidadConError();
        }

        [HttpGet]
        [ConToken]
        [EsOperador]
        [Route("CalcularErrores")]
        public void CalcularErrores()
        {
            var usuarioLogeado = GetUsuarioLogeado();
            if (!usuarioLogeado.Ok)
            {
                var resultado = new ResultadoServicio<int>();
                resultado.Error = usuarioLogeado.Error;
            }

             new v1.Rules.WSRules_Inscripcion(usuarioLogeado.Return).CalcularError();
        }
    }
}