using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Internet.v0;
using WS_Internet.v0.FilterAtributtes;

namespace WS_Internet.v1.Controllers
{
    [RoutePrefix("v1/TipoAuto")]
    public class TipoAuto_v1Controller : ApiController
    {
        [HttpGet]
        [ConToken]
        [v1.Controllers.FilterAttributtes.EsOperador]
        [Route("")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_TipoAuto>> Get()
        {
            return RestCall.Call<List<v1.Entities.Resultados.ResultadoWS_TipoAuto>>(Request);
        }
    }
}