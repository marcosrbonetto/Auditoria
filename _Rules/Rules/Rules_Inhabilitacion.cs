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
                    var validarComando = ValidarComandoInsertarActualizar(comando);
                    if (!validarComando.Ok)
                    {
                        resultado.Error = validarComando.Error;
                        return false;
                    }

                    //Busco el usuario
                    var resultadoUsuario = new Rules_Usuario(getUsuarioLogueado()).GetById(comando.IdUsuario.Value);
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
                    DateTime? fechaInicio = Utils.StringToDate(comando.FechaInicio);
                    DateTime? fechaFin = Utils.StringToDate(comando.FechaFin);


                    //Tipo
                    var resultadoConsultaTipo = new Rules.Rules_TipoInhabilitacion(getUsuarioLogueado()).GetByKeyValue(comando.TipoInhabilitacionKeyValue.Value);
                    if (!resultadoConsultaTipo.Ok)
                    {
                        resultado.Error = resultadoConsultaTipo.Error;
                        return false;
                    }

                    if (resultadoConsultaTipo.Return == null)
                    {
                        resultado.Error = "El tipo inhabilitacion no existe";
                        return false;
                    }


                    var tipo = resultadoConsultaTipo.Return;


                    if (tipo.Invalido)
                    {
                        resultado.Error = "El tipo de inhabilitación es inválido. Seleccione otro por favor";
                        return false;
                    }

                    if (!tipo.Permanente && !fechaFin.HasValue)
                    {
                        resultado.Error = "Si selecciona el tipo " + tipo.Nombre + " debe seleccionar una fecha de fin";
                        return false;
                    }

                    //Creo la entidad
                    var entity = new Inhabilitacion()
                    {
                        Usuario = usuario,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        DtoRes = comando.DtoRes,
                        Expediente = comando.Expediente,
                        Observaciones = comando.Observaciones,
                        ObservacionesTipoAuto = comando.ObservacionesTipoAuto,
                        ObservacionesAutoChapa = comando.ObservacionesAutoChapa,
                        TipoInhabilitacion = tipo
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
                    var validarComando = ValidarComandoInsertarActualizar(comando);
                    if (!validarComando.Ok)
                    {
                        resultado.Error = validarComando.Error;
                        return false;
                    }

                    //Busco el usuario
                    var resultadoUsuario = new Rules_Usuario(getUsuarioLogueado()).GetById(comando.IdUsuario.Value);
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
                    DateTime? fechaInicio = Utils.StringToDate(comando.FechaInicio);
                    DateTime? fechaFin = Utils.StringToDate(comando.FechaFin);

                    //Tipo
                    var resultadoConsultaTipo = new Rules.Rules_TipoInhabilitacion(getUsuarioLogueado()).GetByKeyValue(comando.TipoInhabilitacionKeyValue.Value);
                    if (!resultadoConsultaTipo.Ok)
                    {
                        resultado.Error = resultadoConsultaTipo.Error;
                        return false;
                    }

                    if (resultadoConsultaTipo.Return == null)
                    {
                        resultado.Error = "El tipo inhabilitacion no existe";
                        return false;
                    }


                    var tipo = resultadoConsultaTipo.Return;
                    if (tipo.Invalido)
                    {
                        resultado.Error = "El tipo de inhabilitación es inválido. Seleccione otro por favor";
                        return false;
                    }

                    if (!tipo.Permanente && !fechaFin.HasValue)
                    {
                        resultado.Error = "Si selecciona el tipo " + tipo.Nombre + " debe seleccionar una fecha de fin";
                        return false;
                    }

                    //Busco la entidad
                    var resultadoEntity = GetByIdObligatorio(comando.Id.Value);
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
                    entity.TipoInhabilitacion = tipo;
                    entity.Usuario = usuario;
                    entity.FechaInicio = fechaInicio;
                    entity.FechaFin = fechaFin;
                    entity.DtoRes = comando.DtoRes;
                    entity.Expediente = comando.Expediente;
                    entity.Observaciones = comando.Observaciones;
                    entity.ObservacionesAutoChapa = comando.ObservacionesAutoChapa;
                    entity.ObservacionesTipoAuto = comando.ObservacionesTipoAuto;
                    entity.FechaFinString = null;
                    entity.FechaInicioString = null;
                    entity.Error = null;

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

        public Resultado<bool> ValidarComandoInsertarActualizar(_Model.Comandos.Comando_InhabilitacionNuevo comando)
        {
            var resultado = new Resultado<bool>();

            try
            {

                if (comando is _Model.Comandos.Comando_InhabilitacionActualizar)
                {
                    _Model.Comandos.Comando_InhabilitacionActualizar c = (_Model.Comandos.Comando_InhabilitacionActualizar)comando;
                    if (!c.Id.HasValue || c.Id.Value <= 0)
                    {
                        resultado.Error = "El id de la inhabilitacion es requerido";
                        return resultado;
                    }
                }

                //Usuario
                if (!comando.IdUsuario.HasValue || comando.IdUsuario <= 0)
                {
                    resultado.Error = "El usuario es requerido";
                    return resultado;
                }

                //Tipo
                if (!comando.TipoInhabilitacionKeyValue.HasValue)
                {
                    resultado.Error = "El tipo de inhabilitacion es requerido";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.TipoInhabilitacion), comando.TipoInhabilitacionKeyValue))
                {
                    resultado.Error = "Tipo inhabilitación inválido";
                    return resultado;
                }

                //Fecha de inicio
                if (string.IsNullOrEmpty(comando.FechaInicio))
                {
                    resultado.Error = "La fecha de inicio es requerido";
                    return resultado;
                }

                if (!Utils.StringToDate(comando.FechaInicio).HasValue)
                {
                    resultado.Error = "Fecha de inicio inválida";
                    return resultado;
                }

                //fecha de fin
                if (!string.IsNullOrEmpty(comando.FechaFin) && !Utils.StringToDate(comando.FechaFin).HasValue)
                {
                    resultado.Error = "Fecha de fin inválida";
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

       
    }
}