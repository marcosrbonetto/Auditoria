using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Internet.v0;
using WS_Internet.v0.FilterAtributtes;
using WS_Internet.v1.Controllers.FilterAttributtes;

namespace WS_Internet.v1.Controllers
{
    [RoutePrefix("v1/Inscripcion")]
    public class Inscripcion_v1Controller : ApiController
    {

        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("BuscarPaginado")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InscripcionListado>> BuscarPaginado(v1.Entities.Consultas.Consulta_InscripcionPaginada consulta)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InscripcionListado>>(Request, consulta);
        }

        [HttpGet]
        [ConToken]
        [EsOperador]
        [Route("Detalle")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> GetDetalle(int id)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>(Request);
        }

        [HttpPost]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> Insertar(v1.Entities.Comandos.ComandoWS_InscripcionNuevo comando)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>(Request, comando);
        }

        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> Actualizar(v1.Entities.Comandos.ComandoWS_InscripcionActualizar comando)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>(Request, comando);
        }

        [HttpDelete]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<bool> Borrar(int id)
        {
            return RestCall.Call<bool>(Request);
        }
    }
}