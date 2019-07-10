using System;
using System.Web.Http;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Configuration;
using WS_Internet.v0.FilterAtributtes;

namespace WS_Internet.App
{
    public class SwaggerConfig
    {
        private const string ASSEMBLY_NAME = "WS_Internet";

        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.PrettyPrint();
                    c.SingleApiVersion("v1", "Auditoria - v1");
                    c.OperationFilter<AddRequiredHeaderParameter>();
                })
                .EnableSwaggerUi(c =>
                {
                    var thisAssembly = typeof(SwaggerConfig).Assembly;

                    c.DocumentTitle(ASSEMBLY_NAME + " Auditoria");
                    c.InjectStylesheet(thisAssembly, ASSEMBLY_NAME + ".Resources.Swagger.css");
                    c.InjectJavaScript(thisAssembly, ASSEMBLY_NAME + ".Resources.Swagger.js");
                    //c.EnableDiscoveryUrlSelector();
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
                parametros.Add(new Parameter
                {
                    name = "--Clave",
                    @in = "header",
                    type = "string",
                    description = "Clave",
                    required = false
                });
            }
        }
    }
}