using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System.Collections.Generic;

namespace WS_Internet.v0
{
    public class RestCall
    {
        public static ResultadoServicio<T> Call<T>(HttpRequestMessage httpRequestMessage, object body = null)
        {
            try
            {
                var client = ApiClient(httpRequestMessage.RequestUri.PathAndQuery);
                var request = ApiRequest(httpRequestMessage.Method.Method);

                if (client == null || request == null)
                {
                    return ErrorDefault<T>();
                }

                // Headers
                foreach (var header in httpRequestMessage.Headers)
                {
                    if (header.Value != null && header.Value.FirstOrDefault() != null)
                    {
                        if (header.Key.StartsWith("--"))
                        {
                            foreach (var valor in header.Value)
                            {
                                request.AddHeader(header.Key, valor);
                            }
                        }
                    }
                }

                if (body != null)
                {
                    request.AddBody(body);
                }

                IRestResponse response = client.Execute(request).Result;
                var json = JObject.Parse(response.Content);
                return json.ToObject<ResultadoServicio<T>>();
            }
            catch (Exception e)
            {
                return ErrorDefault<T>();
            }
        }

        private static RestClient ApiClient(string url)
        {
            var directorios = ConfigurationManager.AppSettings["DIRECTORIO_PUBLICACION"].Split(';');

            foreach (var d in directorios)
            {
                if (url.StartsWith("/" + d))
                {
                    url = url.Replace(d + "/", "");
                }
            }

            return new RestClient(ConfigurationManager.AppSettings["URL_SERVER"] + url);
        }

        private static RestRequest ApiRequest(string method)
        {
            Method methodPortable;
            Enum.TryParse(method, true, out methodPortable);

            var request = new RestRequest(methodPortable);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");

            return request;
        }

        private static ResultadoServicio<T> ErrorDefault<T>()
        {
            var resultado = new ResultadoServicio<T>();
            resultado.Error = "Error procesando la solicitud";
            return resultado;
        }
    }
}