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
        private readonly Rules_Usuario _UsuarioRules;

        public Rules_Inhabilitacion(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_Inhabilitacion.Instance;
            _UsuarioRules = new Rules_Usuario(data);
        }

        public Resultado<_Model.Resultados.Resultado_Paginador<Inhabilitacion>> GetPaginado(_Model.Consultas.Consulta_InhabilitacionPaginada consulta)
        {
            return dao.GetPaginado(consulta);
        }

        public Resultado<List<Inhabilitacion>> Get(_Model.Consultas.Consulta_Inhabilitacion consulta)
        {
            return dao.Get(consulta);
        }

        public Resultado<int> GetCantidad(_Model.Consultas.Consulta_Inhabilitacion consulta)
        {
            return dao.GetCantidad(consulta);
        }

        public Resultado<Inhabilitacion> Insertar(_Model.Comandos.Comando_InhabilitacionNuevo comando)
        {
            var resultado = new Resultado<Inhabilitacion>();

            var resultadoTransaccion = dao.Transaction(() =>
            {
                try
                {
                    var validarComando = ValidarComandoInsertar(comando);
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
                    //var validarComando = ValidarComandoInsertarActualizar(comando);
                    //if (!validarComando.Ok)
                    //{
                    //    resultado.Error = validarComando.Error;
                    //    return false;
                    //}


                    if (!comando.Id.HasValue)
                    {
                        resultado.Error = "Indique la inhabilitacion a modificar";
                        return false;
                    }

                    //Busco el usuario
                    Usuario usuario = null;
                    if (comando.IdUsuario.HasValue)
                    {
                        var resultadoUsuario = new Rules_Usuario(getUsuarioLogueado()).GetById(comando.IdUsuario.Value);
                        if (!resultadoUsuario.Ok)
                        {
                            resultado.Error = resultadoUsuario.Error;
                            return false;
                        }

                        usuario = resultadoUsuario.Return;
                        if (usuario == null || usuario.FechaBaja != null)
                        {
                            resultado.Error = "El usuario no existe o esta dado de baja";
                            return false;
                        }
                    }

                    //Valido las fechas
                    DateTime? fechaInicio = null;
                    if (comando.FechaInicio != null && comando.FechaInicio.Trim() != "")
                    {
                        fechaInicio = Utils.StringToDate(comando.FechaInicio.Trim());
                        if (!fechaInicio.HasValue)
                        {
                            resultado.Error = "Fecha de inicio inválida";
                            return false;
                        }
                    }

                    DateTime? fechaFin = null;
                    if (comando.FechaFin != null && comando.FechaFin.Trim() != "")
                    {
                        fechaFin = Utils.StringToDate(comando.FechaFin.Trim());
                        if (!fechaFin.HasValue)
                        {
                            resultado.Error = "Fecha de fin inválida";
                            return false;
                        }
                    }

                    //Tipo
                    TipoInhabilitacion tipo = null;
                    if (comando.TipoInhabilitacionKeyValue.HasValue)
                    {
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

                        tipo = resultadoConsultaTipo.Return;
                        if (tipo == null || tipo.FechaBaja != null)
                        {
                            resultado.Error = "El tipo inhabilitacion no existe";
                            return false;
                        }
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
                        resultado.Error = "La inhabilitacion no existe o esta dado de baja";
                        return false;
                    }

                    //Actualizo
                    entity.TipoInhabilitacion = tipo;
                    entity.Usuario = usuario;
                    entity.FechaInicio = fechaInicio;
                    entity.FechaFin = fechaFin;
                    entity.DtoRes = comando.DtoRes == null || comando.DtoRes.Trim() == "" ? null : comando.DtoRes.Trim();
                    entity.Expediente = comando.Expediente == null || comando.Expediente.Trim() == "" ? null : comando.Expediente.Trim();
                    entity.Observaciones = comando.Observaciones == null || comando.Observaciones.Trim() == "" ? null : comando.Observaciones.Trim();
                    entity.ObservacionesAutoChapa = comando.ObservacionesAutoChapa == null || comando.ObservacionesAutoChapa.Trim() == "" ? null : comando.ObservacionesAutoChapa.Trim();
                    entity.ObservacionesTipoAuto = comando.ObservacionesTipoAuto == null || comando.ObservacionesTipoAuto.Trim() == "" ? null : comando.ObservacionesTipoAuto.Trim();
                    entity.FechaFinString = fechaFin.HasValue ? null : entity.FechaFinString;
                    entity.FechaInicioString = fechaInicio.HasValue ? null : entity.FechaInicioString;

                    //Errores
                    entity.Error = CalcularError(entity);

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

        public Resultado<bool> ToggleFavorito(int id)
        {
            var resultado = new Resultado<bool>();

            var resultadoTransaccion = dao.Transaction(() =>
            {
                try
                {
                    var resultadoEntity = GetByIdObligatorio(id);
                    if (!resultadoEntity.Ok)
                    {
                        resultado.Error = resultadoEntity.Error;
                        return false;
                    }
                    var entity = resultadoEntity.Return;
                    if (entity == null || entity.FechaBaja != null)
                    {
                        resultado.Error = "La inhabilitación no existe o esta dado de baja";
                        return false;
                    }

                    entity.Favorito = !entity.Favorito;

                    //Actualizo
                    var resultadoUpdate = base.Update(entity);
                    if (!resultadoUpdate.Ok)
                    {
                        resultado.Error = resultadoUpdate.Error;
                        return false;
                    }

                    resultado.Return = true;
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

        public Resultado<bool> ValidarComandoInsertar(_Model.Comandos.Comando_InhabilitacionNuevo comando)
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
                if (comando.FechaInicio == null || comando.FechaInicio.Trim() == "")
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
                if (!(comando.FechaFin == null || comando.FechaFin.Trim() == "") && !Utils.StringToDate(comando.FechaFin).HasValue)
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

        public string CalcularError(Inhabilitacion entity)
        {
            List<string> errores = new List<string>();

            //Tipo
            if (entity.TipoInhabilitacion == null)
            {
                errores.Add("Tipo de inhabilitacion requerido");
            }
            else
            {

                //Tipo invalido
                if (entity.TipoInhabilitacion.Invalido)
                {
                    errores.Add("Tipo de inhabilitacion invalido");
                }
                else
                {
                    //Fecha de fin solo cuando es permanente
                    if (!entity.TipoInhabilitacion.Permanente && !entity.FechaFin.HasValue)
                    {
                        errores.Add("Fecha fin requerido");
                    }
                }
            }

            //fecha inicio
            if (!entity.FechaInicio.HasValue)
            {
                errores.Add("Fecha inicio requerido");
            }

            //Usuario
            if (entity.Usuario == null)
            {
                errores.Add("Persona asociada requerida");
            }
            else
            {
                var errorUsuario = new Rules_Usuario(getUsuarioLogueado()).CalcularError(entity.Usuario);
                if (errorUsuario != null)
                {
                    errores.Add("Persona con errores (" + errorUsuario + ")");
                }
            }


            if (errores.Count == 0) return null;
            return string.Join(" | ", errores);
        }

        public Resultado<bool> EstaInhabilitado(int? dni)
        {
            var result = new Resultado<bool>();

            if (!dni.HasValue)
            {
                result.Error = "Dni requerido";
                return result;
            }

            var resultadoInhab = dao.GetCantidadInhabilitaciones(new _Model.Consultas.Consulta_Inhabilitacion
            {
                Dni = dni,
                DadosDeBaja = null
            });
            if (!resultadoInhab.Ok)
            {
                result.Error = resultadoInhab.Error;
                return result;
            }

            result.Return = resultadoInhab.Return > 0 ? true : false;

            return result;
        }

    }
}