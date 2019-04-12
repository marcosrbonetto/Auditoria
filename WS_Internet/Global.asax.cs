using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using WS_Internet.App;


namespace WS_Internet
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

            //var proyectoWsInstranet = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("WS_Intranet")).FirstOrDefault();
            //foreach (var type in proyectoWsInstranet.GetTypes())
            //{
            //    if (type.FullName.StartsWith("WS_Intranet.v1.Entities.Resultados"))
            //    {
                   
            //    }
            //}

            //var referencias = Assembly.GetExecutingAssembly().get;
            //foreach (var assemblyName in referencias)
            //{
            //    Assembly asm = Assembly.Load(assemblyName);
            //    var a = asm.GetTypes().ToList();
            //}
            //.Where(type => type.Namespace == "WS_Instranet")


            //var model = typeof(class).Assembly.GetTypes();
            //foreach (var clase in model)
            //{
            //    if (clase.FullName.StartsWith("WS_Intranet.v1.Entities.Resultados"))
            //    {
            //    }
            //}
            //(from a in AppDomain.CurrentDomain.GetAssemblies() select a into assemblies select assemblies)
            //    .ToList()
            //    .ForEach(a =>
            //    {
            //        if (a.FullName.StartsWith("WS_Intranet.v1.Entities.Resultados"))
            //        {
            //            foreach (var type in a.DefinedTypes)
            //            {
            //                var asdasd = 1;
            //            }
            //        }
            //    });

            SwaggerConfig.Register();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}