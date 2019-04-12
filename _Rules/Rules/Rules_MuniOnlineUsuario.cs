using System;
using System.Linq;
using _DAO.DAO;
using _Model;
using _Model.Entities;
using System.Configuration;
using System.Collections.Generic;
using _Rules._WsVecinoVirtual;

namespace _Rules.Rules
{
    public class Rules_MuniOnlineUsuario : BaseRules<MuniOnlineUsuario>
    {
        private readonly DAO_MuniOnlineUsuario dao;

        private readonly string baseUrl = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"];
        //private readonly string baseUrl = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL_TEST"];

        public Rules_MuniOnlineUsuario(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_MuniOnlineUsuario.Instance;
        }

        public Resultado<List<MuniOnlineUsuario>> Get(_Model.Consultas.Consulta_MuniOnlineUsuario consulta)
        {
            return dao.Get(consulta);
        }


        public Resultado<string> IniciarSesion(string username, string password)
        {
            var resultado = new Resultado<string>();

            try
            {
                var url = baseUrl + "/v2/Usuario/IniciarSesion";
                var headers = new Dictionary<string, string>();
                headers["--Username"] = username;
                headers["--Password"] = password;
                var resultadoIniciarSesion = RestCall.Call<string>(url, RestSharp.Portable.Method.GET, null, headers);

                if (!resultadoIniciarSesion.Ok)
                {
                    resultado.Error = resultadoIniciarSesion.Error;
                    return resultado;
                }


                resultado.Return = resultadoIniciarSesion.Return;
                return resultado;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }

            return resultado;
        }

        public Resultado<bool> CerrarSesion(string token)
        {
            var resultado = new Resultado<bool>();

            try
            {
                var url = baseUrl + "/v1/Usuario/CerrarSesion?token=" + token;
                var resultadoCerrarSesion = RestCall.Call<bool?>(url, RestSharp.Portable.Method.PUT);
                if (!resultadoCerrarSesion.Ok)
                {
                    resultado.Error = resultadoCerrarSesion.Error;
                    return resultado;
                }

                resultado.Return = true;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }

            return resultado;
        }

        public Resultado<int> GetIdByToken(string token)
        {
            var resultado = new Resultado<int>();

            try
            {
                var urlId = baseUrl + "/v1/Usuario/GetId?token=" + token;

                var resultadoConsultaId = RestCall.Call<int?>(urlId, RestSharp.Portable.Method.GET);
                if (!resultadoConsultaId.Ok)
                {
                    resultado.Error = resultadoConsultaId.Error;
                    return resultado;
                }


                if (!resultadoConsultaId.Return.HasValue || resultadoConsultaId.Return.Value == 0)
                {
                    resultado.Error = "El usuario no existe";
                    return resultado;
                }
                int id = resultadoConsultaId.Return.Value;
                resultado.Return = id;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }

            return resultado;
        }

        public Resultado<MuniOnlineUsuario> GetByToken(string token)
        {
            var resultado = new Resultado<MuniOnlineUsuario>();

            try
            {
                var resultadoId = GetIdByToken(token);
                if (!resultadoId.Ok)
                {
                    resultado.Error = resultadoId.Error;
                    return resultado;
                }

                var resultadoValidadoRenaper = ValidadoRenaper(token);
                if (!resultadoValidadoRenaper.Ok)
                {
                    resultado.Error = resultadoValidadoRenaper.Error;
                    return resultado;
                }
                if (resultadoValidadoRenaper.Return == false)
                {
                    resultado.Error = "El usuario no esta validado por renaper";
                    return resultado;
                }

                return GetByIdObligatorio(resultadoId.Return);
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }

            return resultado;
        }

        public Resultado<bool> ValidarToken(string token)
        {
            var resultado = new Resultado<bool>();

            try
            {
                var url = baseUrl + "/v1/Usuario/ValidarToken?token=" + token;

                var resultadoValidarToken = RestCall.Call<bool?>(url, RestSharp.Portable.Method.GET);
                if (!resultadoValidarToken.Ok)
                {
                    resultado.Error = resultadoValidarToken.Error;
                    return resultado;
                }

                resultado.Return = resultadoValidarToken.Return.Value;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }

            return resultado;
        }

        public Resultado<bool> ValidadoRenaper(string token)
        {
            var resultado = new Resultado<bool>();

            try
            {
                var url = baseUrl + "/v1/Usuario/ValidadoRenaper?token=" + token;
                var resultadoValidadoRenaper = RestCall.Call<bool?>(url, RestSharp.Portable.Method.GET);
                if (!resultadoValidadoRenaper.Ok)
                {
                    resultado.Error = resultadoValidadoRenaper.Error;
                    return resultado;
                }

                resultado.Return = resultadoValidadoRenaper.Return.Value;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }

            return resultado;
        }

        public Resultado<bool> EsOperador(int idUsuario)
        {
            var resultado = new Resultado<bool>();

            try
            {
                var resultadoQuery = dao.ProcedimientoAlmacenado<int>("EsOperador @idUsuario=" + idUsuario);
                if (!resultadoQuery.Ok)
                {
                    resultado.Error = resultadoQuery.Error;
                    return resultado;
                }

                resultado.Return = resultadoQuery.Return[0] != 0;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }

            return resultado;
        }


    }
}