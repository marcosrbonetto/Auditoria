using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using WS_Intranet.v0.Controllers.FilterAtributtes;

namespace WS_Intranet.v1.Controllers.FilterAtributtes
{
    public class ConClaveInterna : _Autorizacion
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            //Token
            if (!actionContext.Request.Headers.Contains("--Token"))
            {
                actionContext.Response = Error(HttpStatusCode.OK, "Debe mandar su token de acceso de Vecino Virtual");
                return;
            }
            var token = actionContext.Request.Headers.GetValues("--Token").FirstOrDefault();
            var resultadoId = new _Rules.Rules.Rules_MuniOnlineUsuario(null).GetIdByToken(token);
            if (!resultadoId.Ok)
            {
                actionContext.Response = Error(HttpStatusCode.OK, resultadoId.Error);
                return;
            }

            //Clave
            if (!actionContext.Request.Headers.Contains("--Clave"))
            {
                actionContext.Response = Error(HttpStatusCode.OK, "Debe mandar su clave de acceso");
                return;
            }
            var clave = actionContext.Request.Headers.GetValues("--Clave").FirstOrDefault();
            var resultadoClave = new _Rules.Rules.Rules_MuniOnlineUsuario(null).EsClaveCorrecta(clave);
            if (!resultadoClave.Ok || !resultadoClave.Return)
            {
                actionContext.Response = Error(HttpStatusCode.OK, "No tiene el permiso necesario para realizar esta acción");
                return;
            }
        }
    }
}