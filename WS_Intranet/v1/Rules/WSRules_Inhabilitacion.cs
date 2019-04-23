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
    public class WSRules_Inhabilitacion : WSRules_Base
    {

        public WSRules_Inhabilitacion(UsuarioLogueado data)
            : base(data)
        {
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InhabilitacionListado>> BuscarPaginado(v1.Entities.Consultas.Consulta_InhabilitacionPaginada consulta)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InhabilitacionListado>>();

            //Busco la info
            var resultadoData = new _Rules.Rules.Rules_Inhabilitacion(getUsuarioLogueado()).GetPaginado(consulta.Convertir());
            if (!resultadoData.Ok)
            {
                resultado.Error = resultadoData.Error;
                return resultado;
            }

            //Resultado
            resultado.Return = new ResultadoWS_Paginador<ResultadoWS_InhabilitacionListado>();
            resultado.Return.Count = resultadoData.Return.Count;
            resultado.Return.CantidadPaginas = resultadoData.Return.CantidadPaginas;
            resultado.Return.OrderBy = resultadoData.Return.OrderBy;
            resultado.Return.PaginaActual = resultadoData.Return.PaginaActual;
            resultado.Return.TamañoPagina = resultadoData.Return.TamañoPagina;
            resultado.Return.Data = v1.Entities.Resultados.ResultadoWS_InhabilitacionListado.ToList(resultadoData.Return.Data);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> GetDetalle(int id)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Inhabilitacion(getUsuarioLogueado()).GetById(id);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Valido
            var entity = resultadoQuery.Return;
            if (entity == null || entity.FechaBaja != null)
            {
                resultado.Error = "La inhabilitación no existe o está dada de baja";
                return resultado;
            }


            //Convierto
            resultado.Return = new ResultadoWS_InhabilitacionDetalle(resultadoQuery.Return);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> Insertar(v1.Entities.Comandos.ComandoWS_InhabilitacionNuevo comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Inhabilitacion(getUsuarioLogueado()).Insertar(comando.Convertir());
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Convierto
            resultado.Return = new ResultadoWS_InhabilitacionDetalle(resultadoQuery.Return);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle> Actualizar(v1.Entities.Comandos.ComandoWS_InhabilitacionActualizar comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InhabilitacionDetalle>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Inhabilitacion(getUsuarioLogueado()).Actualizar(comando.Convertir());
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Convierto
            resultado.Return = new ResultadoWS_InhabilitacionDetalle(resultadoQuery.Return);
            return resultado;
        }

        public ResultadoServicio<bool> ToggleFavorito(int id)
        {
            var resultado = new ResultadoServicio<bool>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Inhabilitacion(getUsuarioLogueado()).ToggleFavorito(id);
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
            var resultadoQuery = new _Rules.Rules.Rules_Inhabilitacion(getUsuarioLogueado()).Borrar(id);
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
            var resultadoQuery = new _Rules.Rules.Rules_Inhabilitacion(getUsuarioLogueado()).GetCantidad(new _Model.Consultas.Consulta_Inhabilitacion()
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