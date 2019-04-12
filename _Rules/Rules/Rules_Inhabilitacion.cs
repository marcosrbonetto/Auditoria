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
    public class Rules_Inhabilitacion : BaseRules<Inhabilitacion>
    {
        private readonly DAO_Inhabilitacion dao;

        public Rules_Inhabilitacion(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_Inhabilitacion.Instance;
        }

        public Resultado<_Model.Resultados.Resultado_Paginador<Inhabilitacion>> GetPaginado(_Model.Consultas.Consulta_InhabilitacionPaginada consulta)
        {
            return dao.GetPaginado(consulta);
        }

        public Resultado<List<Inhabilitacion>> Get(_Model.Consultas.Consulta_Inhabilitacion consulta)
        {
            return dao.Get(consulta);
        }

        public Resultado<Inhabilitacion> Insertar(_Model.Comandos.Comando_InhabilitacionNuevo comando)
        {
            var resultado = new Resultado<Inhabilitacion>();

            var resultadoTransaccion = dao.Transaction(() =>
            {
                try
                {
                    //Busco el usuario
                    var resultadoUsuario = new Rules_Usuario(getUsuarioLogueado()).GetById(comando.IdUsuario);
                    if (!resultadoUsuario.Ok)
                    {
                        resultado.Error = resultadoUsuario.Error;
                        return false;
                    }

                    var usuario = resultadoUsuario.Return;
                    if (usuario == null || usuario.FechaBaja != null)
                    {
                        resultado.Error = "El usuario no existe o esta dado de baja";
                        return false;
                    }

                    //Valido las fechas
                    DateTime? fechaInicio = null;
                    DateTime? fechaFin = null;
                    DateTime? fechaExpediente = null;

                    if (!string.IsNullOrEmpty(comando.FechaInicio))
                    {
                        fechaInicio = Utils.StringToDate(comando.FechaInicio);
                        if (fechaInicio == null)
                        {
                            resultado.Error = "El formato de la fecha de inicio es inválida";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(comando.FechaFin))
                    {
                        fechaFin = Utils.StringToDate(comando.FechaFin);
                        if (fechaFin == null)
                        {
                            resultado.Error = "El formato de la fecha de inicio es inválida";
                            return false;
                        }
                    }

                    //Creo la entidad
                    var entity = new Inhabilitacion()
                    {
                        Usuario = usuario,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        DtoRes = comando.DtoRes,
                        Expediente = comando.Expediente,
                        Observaciones = comando.Observaciones
                    };

                    //Inserto
                    var resultadoInsertar = base.Insert(entity);
                    if (!resultadoInsertar.Ok)
                    {
                        resultado.Error = resultadoInsertar.Error;
                        return false;
                    }

                    resultado.Return = resultadoInsertar.Return;
                    return true;
                }
                catch (Exception e)
                {
                    resultado.Error = "Error procesando la solicitud";
                    return false;
                }
            });

            if (resultado.Ok && !resultadoTransaccion)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            return resultado;
        }

        public Resultado<Inhabilitacion> Actualizar(_Model.Comandos.Comando_InhabilitacionActualizar comando)
        {
            var resultado = new Resultado<Inhabilitacion>();

            var resultadoTransaccion = dao.Transaction(() =>
            {
                try
                {
                    //Busco el usuario
                    var resultadoUsuario = new Rules_Usuario(getUsuarioLogueado()).GetById(comando.IdUsuario);
                    if (!resultadoUsuario.Ok)
                    {
                        resultado.Error = resultadoUsuario.Error;
                        return false;
                    }

                    var usuario = resultadoUsuario.Return;
                    if (usuario == null || usuario.FechaBaja != null)
                    {
                        resultado.Error = "El usuario no existe o esta dado de baja";
                        return false;
                    }

                    //Valido las fechas
                    DateTime? fechaInicio = null;
                    DateTime? fechaFin = null;

                    if (!string.IsNullOrEmpty(comando.FechaInicio))
                    {
                        fechaInicio = Utils.StringToDate(comando.FechaInicio);
                        if (fechaInicio == null)
                        {
                            resultado.Error = "El formato de la fecha de inicio es inválida";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(comando.FechaFin))
                    {
                        fechaFin = Utils.StringToDate(comando.FechaFin);
                        if (fechaFin == null)
                        {
                            resultado.Error = "El formato de la fecha de inicio es inválida";
                            return false;
                        }
                    }

                    //Busco la entidad
                    var resultadoEntity = GetByIdObligatorio(comando.Id);
                    if (!resultadoEntity.Ok)
                    {
                        resultado.Error = resultadoEntity.Error;
                        return false;
                    }

                    var entity = resultadoEntity.Return;
                    if (entity == null || entity.FechaBaja != null)
                    {
                        resultado.Error = "El titular no existe o esta dado de baja";
                        return false;
                    }

                    //Actualizo
                    entity.Usuario = usuario;
                    entity.FechaInicio = fechaInicio;
                    entity.FechaFin = fechaFin;
                    entity.DtoRes = comando.DtoRes;
                    entity.Expediente = comando.Expediente;
                    entity.Observaciones = comando.Observaciones;

                    //Actualizo
                    var resultadoUpdate = base.Update(entity);
                    if (!resultadoUpdate.Ok)
                    {
                        resultado.Error = resultadoUpdate.Error;
                        return false;
                    }

                    resultado.Return = resultadoUpdate.Return;
                    return true;

                }
                catch (Exception e)
                {
                    resultado.Error = "Error procesando la solicitud";
                    return false;
                }
            });

            if (resultado.Ok && !resultadoTransaccion)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            return resultado;
        }

        public Resultado<bool> Borrar(int id)
        {
            var resultado = new Resultado<bool>();

            var resultadoQuery = GetById(id);
            if (!resultadoQuery.Ok)
            {
                resultado.Error = resultadoQuery.Error;
                return resultado;
            }

            var entity = resultadoQuery.Return;
            if (entity == null || entity.FechaBaja != null)
            {
                resultado.Error = "La inhabilitacion no existe o está dada de baja";
                return resultado;
            }

            var resultadoDelete = base.Delete(entity);
            if (!resultadoDelete.Ok)
            {
                resultado.Error = resultadoDelete.Error;
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }
    }
}