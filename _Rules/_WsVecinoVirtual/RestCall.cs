using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Linq;
using System.Collections.Generic;

namespace _Rules._WsVecinoVirtual
{
    public class RestCall
    {
        public static RestClient ApiClient(string url)
        {
            var client = new RestClient(url);
            return client;
        }

        public static RestRequest ApiRequest(Method method)
        {
            var request = new RestRequest(method);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            return request;
        }

        public static VVResult<T> Call<T>(string url, Method metodo, object body = null, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = ApiClient(url);
                var request = ApiRequest(metodo);
                if (client == null || request == null)
                {
                    var resultado = new VVResult<T>();
                    resultado.Error = "Error procesando la solicitud";
                    return resultado;
                }

                if (body != null)
                {
                    request.AddBody(body);
                }

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                IRestResponse response = client.Execute(request).Result;
                var json = JObject.Parse(response.Content);

                //Devuelvo
                return json.ToObject<VVResult<T>>();

            }
            catch (Exception e)
            {
                var resultado = new VVResult<T>();
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }
        }
    }
}