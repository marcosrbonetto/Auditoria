using System;
using System.Linq;
using WS_Intranet.v1.Entities.Resultados;
using _Rules;
using WS_Intranet.v0.Rules;
using WS_Intranet.v0;
using _Rules.Rules;
using System.Collections.Generic;

namespace WS_Intranet.v1.Rules
{
    public class WSRules_MuniOnlineUsuario : WSRules_Base
    {
        private readonly Rules_MuniOnlineUsuario rules;

        public WSRules_MuniOnlineUsuario(UsuarioLogueado data)
            : base(data)
        {
            rules = new Rules_MuniOnlineUsuario(data);
        }


        public ResultadoServicio<string> IniciarSesion(v1.Entities.Comandos.ComandoWS_IniciarSesion comando)
        {
            var resultado = new ResultadoServicio<string>();

            var resultadoIniciarSesion = rules.IniciarSesion(comando.Username, comando.Password);
            if (!resultadoIniciarSesion.Ok)
            {
                resultado.Error = resultadoIniciarSesion.Error;
                return resultado;
            }

            resultado.Return = resultadoIniciarSesion.Return;
            return resultado;
        }

        public ResultadoServicio<bool> CerrarSesion(string token)
        {
            var resultado = new ResultadoServicio<bool>();

            var resultadoCerrarSesion = rules.CerrarSesion(token);
            if (!resultadoCerrarSesion.Ok)
            {
                resultado.Error = resultadoCerrarSesion.Error;
                return resultado;
            }

            resultado.Return = resultadoCerrarSesion.Return;
            return resultado;

        }


        public ResultadoServicio<ResultadoWS_MuniOnlineUsuario> GetUsuario(string token)
        {
            var resultado = new ResultadoServicio<ResultadoWS_MuniOnlineUsuario>();

            var resultadoConsulta = rules.GetByToken(token);
            if (!resultadoConsulta.Ok)
            {
                resultado.Error = resultadoConsulta.Error;
                return resultado;
            }

            resultado.Return = new ResultadoWS_MuniOnlineUsuario(resultadoConsulta.Return);
            return resultado;
        }

        public ResultadoServicio<bool> ValidarToken(string token)
        {
            var resultado = new ResultadoServicio<bool>();

            var resultadoConsulta = rules.ValidarToken(token);
            if (!resultadoConsulta.Ok)
            {
                resultado.Error = resultadoConsulta.Error;
                return resultado;
            }

            resultado.Return = resultadoConsulta.Return;
            return resultado;
        }

        public ResultadoServicio<bool> ValidadoRenaper(string token)
        {
            var resultado = new ResultadoServicio<bool>();

            var resultadoConsulta = rules.ValidadoRenaper(token);
            if (!resultadoConsulta.Ok)
            {
                resultado.Error = resultadoConsulta.Error;
                return resultado;
            }

            resultado.Return = resultadoConsulta.Return;
            return resultado;
        }

        public ResultadoServicio<bool> EsOperador(int idUsuario)
        {
            var resultado = new ResultadoServicio<bool>();

            var resultadoConsulta = rules.EsOperador(idUsuario);
            if (!resultadoConsulta.Ok)
            {
                resultado.Error = resultadoConsulta.Error;
                return resultado;
            }

            resultado.Return = resultadoConsulta.Return;
            return resultado;
        }
    }
}