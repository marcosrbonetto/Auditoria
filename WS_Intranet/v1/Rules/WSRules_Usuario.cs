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
    public class WSRules_Usuario : WSRules_Base
    {
        private readonly Rules_Usuario rules;

        public WSRules_Usuario(UsuarioLogueado data)
            : base(data)
        {
            rules = new Rules_Usuario(data);
        }


        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_Usuario>> BuscarPaginado(v1.Entities.Consultas.Consulta_UsuarioPaginado consulta)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_Usuario>>();

            if (!string.IsNullOrEmpty(consulta.FechaNacimiento))
            {
                var fecha = _Model.Utils.StringToDate(consulta.FechaNacimiento);
                if (fecha == null || !fecha.HasValue)
                {
                    resultado.Error = "El formato de la fecha de nacimiento es inválido";
                    return resultado;
                }
            }

            //Busco la info
            var resultadoData = new _Rules.Rules.Rules_Usuario(getUsuarioLogueado()).GetPaginado(consulta.Convertir());
            if (!resultadoData.Ok)
            {
                resultado.Error = resultadoData.Error;
                return resultado;
            }

            //Resultado
            resultado.Return = new ResultadoWS_Paginador<ResultadoWS_Usuario>();
            resultado.Return.Count = resultadoData.Return.Count;
            resultado.Return.CantidadPaginas = resultadoData.Return.CantidadPaginas;
            resultado.Return.OrderBy = resultadoData.Return.OrderBy;
            resultado.Return.PaginaActual = resultadoData.Return.PaginaActual;
            resultado.Return.TamañoPagina = resultadoData.Return.TamañoPagina;
            resultado.Return.Data = v1.Entities.Resultados.ResultadoWS_Usuario.ToList(resultadoData.Return.Data);
            return resultado;
        }

        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_Usuario>> Buscar(v1.Entities.Consultas.Consulta_Usuario consulta)
        {
            var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoWS_Usuario>>();

            var resultadoInsertar = rules.Get(consulta.Convertir());
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Error;
                return resultado;
            }

            resultado.Return = ResultadoWS_Usuario.ToList(resultadoInsertar.Return);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> Insertar(v1.Entities.Comandos.ComandoWS_UsuarioNuevo comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario>();

            var resultadoInsertar = rules.Insertar(comando.Convertir());
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Error;
                return resultado;
            }

            if (resultadoInsertar.Return != null)
            {
                resultado.Return = new ResultadoWS_Usuario(resultadoInsertar.Return);
            }

            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> Actualizar(v1.Entities.Comandos.ComandoWS_UsuarioActualizar comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario>();

            var resultadoInsertar = rules.Actualizar(comando.Convertir());
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Error;
                return resultado;
            }

            if (resultadoInsertar.Return != null)
            {
                resultado.Return = new ResultadoWS_Usuario(resultadoInsertar.Return);
            }

            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario> GetDetalle(int id)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Usuario>();

            var resultadoQuery = rules.GetById(id);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            var entity = resultadoQuery.Return;
            if (entity == null || entity.FechaBaja != null)
            {
                resultado.Error = "El usuario no existe o esta dado de baja";
                return resultado;
            }

            resultado.Return = new ResultadoWS_Usuario(resultadoQuery.Return);
            return resultado;

        }

        public ResultadoServicio<bool> ToggleFavorito(int id)
        {
            var resultado = new ResultadoServicio<bool>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Usuario(getUsuarioLogueado()).ToggleFavorito(id);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Convierto
            resultado.Return = resultadoQuery.Return;
            return resultado;
        }

        public ResultadoServicio<bool> Borrar(int id)
        {
            var resultado = new ResultadoServicio<bool>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Usuario(getUsuarioLogueado()).Borrar(id);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Convierto
            resultado.Return = true;
            return resultado;
        }

        public ResultadoServicio<int> GetCantidadConError()
        {
            var resultado = new ResultadoServicio<int>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Usuario(getUsuarioLogueado()).GetCantidad(new _Model.Consultas.Consulta_Usuario()
            {
                ConError = true,
                DadosDeBaja = false
            });
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Convierto
            resultado.Return = resultadoQuery.Return;
            return resultado;
        }


    }
}