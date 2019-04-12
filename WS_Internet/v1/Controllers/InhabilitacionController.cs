using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Internet.v0;
using WS_Internet.v0.FilterAtributtes;
using WS_Internet.v1.Controllers.FilterAttributtes;

namespace WS_Internet.v1.Controllers
{
    [RoutePrefix("v1/Inhabilitacion")]
    public class Inhabilitacion_v1Controller : ApiController
    {
        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("BuscarPaginado")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InhabilitacionListado>> BuscarPaginado(v1.Entities.Consultas.Consulta_InhabilitacionPaginada consulta)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InhabilitacionListado>>(Request, consulta);
        }

        [HttpGet]
        [ConToken]
        [EsOperador]
        [Route("Detalle")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> GetDetalle(int id)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>(Request);
        }

        [HttpPost]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> Insertar(v1.Entities.Comandos.ComandoWS_InhabilitacionNuevo comando)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>(Request, comando);
        }

        [HttpPut]
        [ConToken]
        [EsOperador]
        [Route("")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> Actualizar(v1.Entities.Comandos.ComandoWS_InhabilitacionActualizar comando)
        {
            return RestCall.Call<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>(Request, comando);
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