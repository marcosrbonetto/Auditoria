using System;
using System.Web.Http;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Configuration;
using WS_Intranet.v0.Controllers.FilterAtributtes;

namespace WS_Intranet.App
{
    public class SwaggerConfig
    {
        private const string ASSEMBLY_NAME = "WS_Intranet";

        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.PrettyPrint();
                        c.SingleApiVersion("WS", "Sorteo Taxis y Remises");
                        c.OperationFilter<AddRequiredHeaderParameter>();
                        //c.IncludeXmlComments(String.Format(@"{0}\bin\\Resources\\Documentacion.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                    })
                .EnableSwaggerUi(c =>
                    {
                        var thisAssembly = typeof(SwaggerConfig).Assembly;

                        c.DocumentTitle(ASSEMBLY_NAME + " Sorteo Taxis y Remises");
                        c.InjectStylesheet(thisAssembly, ASSEMBLY_NAME + ".Resources.Swagger.css");
                        c.InjectJavaScript(thisAssembly, ASSEMBLY_NAME + ".Resources.Swagger.js");
                        c.EnableDiscoveryUrlSelector();
                    });
        }

        public class AddRequiredHeaderParameter : IOperationFilter
        {
            public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
            {
                if (operation.parameters == null)
                {
                    operation.parameters = new List<Parameter>();
                }


                //Token
                var atributoToken = apiDescription.GetControllerAndActionAttributes<ConToken>().GetEnumerator();
                if (atributoToken != null && atributoToken.MoveNext() == true)
                {
                    AddToken(operation.parameters);
                }

                // -----------------------------------------------------
                // V1
                // -----------------------------------------------------
                var v1_atributoEsOperador = apiDescription.GetControllerAndActionAttributes<v1.Controllers.FilterAtributtes.EsOperador>().GetEnumerator();
                if (v1_atributoEsOperador != null && v1_atributoEsOperador.MoveNext() == true)
                {
                    AddToken(operation.parameters);
                }
            }

            private void AddToken(IList<Parameter> parametros)
            {
                List<Parameter> lista = new List<Parameter>(parametros);
                if (lista.Find(x => x.name == "--Token") != null) return;

                parametros.Add(new Parameter
                {
                    name = "--Token",
                    @in = "header",
                    type = "string",
                    description = "Token",
                    required = true
                });
            }
        }
    }
}