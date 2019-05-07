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
    public class WSRules_Inscripcion : WSRules_Base
    {

        public WSRules_Inscripcion(UsuarioLogueado data)
            : base(data)
        {
        }


        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InscripcionListado>> BuscarPaginado(v1.Entities.Consultas.Consulta_InscripcionPaginada consulta)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_Paginador<v1.Entities.Resultados.ResultadoWS_InscripcionListado>>();

            //Busco la info
            var resultadoData = new _Rules.Rules.Rules_Inscripcion(getUsuarioLogueado()).GetPaginado(consulta.Convertir());
            if (!resultadoData.Ok)
            {
                resultado.Error = resultadoData.Error;
                return resultado;
            }

            //Resultado
            resultado.Return = new ResultadoWS_Paginador<ResultadoWS_InscripcionListado>();
            resultado.Return.Count = resultadoData.Return.Count;
            resultado.Return.CantidadPaginas = resultadoData.Return.CantidadPaginas;
            resultado.Return.OrderBy = resultadoData.Return.OrderBy;
            resultado.Return.PaginaActual = resultadoData.Return.PaginaActual;
            resultado.Return.TamañoPagina = resultadoData.Return.TamañoPagina;
            resultado.Return.Data = v1.Entities.Resultados.ResultadoWS_InscripcionListado.ToList(resultadoData.Return.Data);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> GetDetalle(int id)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Inscripcion(getUsuarioLogueado()).GetById(id);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Valido
            var entity = resultadoQuery.Return;
            if (entity == null || entity.FechaBaja != null)
            {
                resultado.Error = "La inscripción no existe o está dada de baja";
                return resultado;
            }


            //Convierto
            resultado.Return = new ResultadoWS_InscripcionDetalle(resultadoQuery.Return);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> Insertar(v1.Entities.Comandos.ComandoWS_InscripcionNuevo comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Inscripcion(getUsuarioLogueado()).Insertar(comando.Convertir());
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Convierto
            resultado.Return = new ResultadoWS_InscripcionDetalle(resultadoQuery.Return);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle> Actualizar(v1.Entities.Comandos.ComandoWS_InscripcionActualizar comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoWS_InscripcionDetalle>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Inscripcion(getUsuarioLogueado()).Actualizar(comando.Convertir());
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            //Convierto
            resultado.Return = new ResultadoWS_InscripcionDetalle(resultadoQuery.Return);
            return resultado;
        }

        public ResultadoServicio<bool> ToggleFavorito(int id)
        {
            var resultado = new ResultadoServicio<bool>();

            //Busco 
            var resultadoQuery = new _Rules.Rules.Rules_Inscripcion(getUsuarioLogueado()).ToggleFavorito(id);
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
            var resultadoQuery = new _Rules.Rules.Rules_Inscripcion(getUsuarioLogueado()).Borrar(id);
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
            var resultadoQuery = new _Rules.Rules.Rules_Inscripcion(getUsuarioLogueado()).GetCantidad(new _Model.Consultas.Consulta_Inscripcion()
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

        public void CalcularError()
        {
            new _Rules.Rules.Rules_Inscripcion(getUsuarioLogueado()).calcularErrores();
        }

    }
}