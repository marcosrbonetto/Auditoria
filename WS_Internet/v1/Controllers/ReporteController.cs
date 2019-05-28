using System;
using System.Linq;
using System.Web.Http;
using WS_Internet.v0;
using WS_Internet.v0.FilterAtributtes;

namespace WS_Internet.v1.Controllers
{
    [RoutePrefix("v1/Reporte")]
    public class Reporte_v1Controller : ApiController
    {
        [HttpPost]
        [v0.FilterAtributtes.ConToken]
        [Route("GetInscripcionesPorDni")]
        public v0.ResultadoServicio<string> GetInscripcionesPorDni(int? dni)
        {
            return v0.RestCall.Call<string>(Request);
        }

        [HttpPost]
        [v0.FilterAtributtes.ConToken]
        [Route("GetInscripcionesPorChapa")]
        public v0.ResultadoServicio<string> GetInscripcionesPorChapa(int? tipoAuto, int? numero)
        {
            return v0.RestCall.Call<string>(Request);
        }
    }
}