using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WS_Internet.v0;
using WS_Internet.v0.FilterAtributtes;
using WS_Internet.v1.Entities.Consultas;

namespace WS_Internet.v1.Controllers
{
    [RoutePrefix("v1/Reporte")]
    public class Reporte_v1Controller : ApiController
    {
        [HttpPost]
        [ConToken]
        [Route("GetInscripcionesPorDni")]
        public ResultadoServicio<string> GetInscripcionesPorDni(Consulta_Inscripcion consulta)
        {
            return RestCall.Call<string>(Request, consulta);
        }

        [HttpPost]
        [ConToken]
        [Route("GetInscripcionesPorChapa")]
        public ResultadoServicio<string> GetInscripcionesPorChapa(Consulta_Inscripcion consulta)
        {
            return RestCall.Call<string>(Request, consulta);
        }
    }
}