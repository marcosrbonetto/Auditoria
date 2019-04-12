using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Internet.v0;
using WS_Internet.v0.FilterAtributtes;

namespace WS_Internet.v1.Controllers
{
    [RoutePrefix("v1/Usuario")]
    public class Usuario_v1Controller : ApiController
    {
        [HttpPost]
        [ConToken]
        [v1.Controllers.FilterAttributtes.EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> Insertar(v1.Entities.Comandos.ComandoWS_UsuarioNuevo comando)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_Usuario>(Request, comando);
        }

        [HttpPut]
        [ConToken]
        [v1.Controllers.FilterAttributtes.EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> Actualizar(v1.Entities.Comandos.ComandoWS_UsuarioActualizar comando)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_Usuario>(Request, comando);
        }

        [HttpDelete]
        [ConToken]
        [v1.Controllers.FilterAttributtes.EsOperador]
        [Route("")]
        public ResultadoServicio<bool> Borrar(int id)
        {
            return RestCall.Call<bool>(Request);
        }


        [HttpPut]
        [ConToken]
        [v1.Controllers.FilterAttributtes.EsOperador]
        [Route("Buscar")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_Usuario>> Buscar(v1.Entities.Consultas.Consulta_Usuario consulta)
        {
            return RestCall.Call<List<v1.Entities.Resultados.ResultadoWS_Usuario>>(Request, consulta);
        }

        [HttpPut]
        [ConToken]
        [v1.Controllers.FilterAttributtes.EsOperador]
        [Route("BuscarPaginado")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_Usuario>> BuscarPaginado(v1.Entities.Consultas.Consulta_UsuarioPaginado consulta)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_Usuario>>(Request, consulta);

        }

        [HttpGet]
        [ConToken]
        [v1.Controllers.FilterAttributtes.EsOperador]
        [Route("Detalle")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> GetDetalle(int id)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_Usuario>(Request);
        }
    }
}