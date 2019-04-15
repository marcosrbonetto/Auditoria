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
    public class Rules_Inscripcion : BaseRules<Inscripcion>
    {
        private readonly DAO_Inscripcion dao;

        public Rules_Inscripcion(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_Inscripcion.Instance;
        }

        public Resultado<_Model.Resultados.Resultado_Paginador<Inscripcion>> GetPaginado(_Model.Consultas.Consulta_InscripcionPaginada consulta)
        {
            return dao.GetPaginado(consulta);
        }

        public Resultado<List<Inscripcion>> Get(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            return dao.Get(consulta);
        }

        public Resultado<Inscripcion> Insertar(_Model.Comandos.Comando_InscripcionNuevo comando)
        {
            var resultado = new Resultado<Inscripcion>();

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

                    //Busco el tipo inscripcion
                    var resultadoTipoInscripcion = new Rules_TipoInscripcion(getUsuarioLogueado()).GetByKeyValue(comando.TipoInscripcionKeyValue);
                    if (!resultadoTipoInscripcion.Ok)
                    {
                        resultado.Error = resultadoTipoInscripcion.Error;
                        return false;
                    }

                    var tipoInscripcion = resultadoTipoInscripcion.Return;
                    if (tipoInscripcion == null || tipoInscripcion.FechaBaja != null)
                    {
                        resultado.Error = "El tipo de inscripción no existe o esta dado de baja";
                        return false;
                    }

                    //Busco el tipo de auto
                    var resultadoTipoAuto = new Rules_TipoAuto(getUsuarioLogueado()).GetByKeyValue(comando.TipoAutoKeyValue);
                    if (!resultadoTipoAuto.Ok)
                    {
                        resultado.Error = resultadoTipoAuto.Error;
                        return false;
                    }
                    var tipoAuto = resultadoTipoAuto.Return;
                    if (tipoAuto == null || tipoAuto.FechaBaja != null)
                    {
                        resultado.Error = "El tipo de auto no existe o esta dado de baja";
                        return false;
                    }

                    //Busco el tipo condicion
                    TipoCondicionInscripcion condicion = null;
                    if (comando.TipoCondicionInscripcionKeyValue.HasValue)
                    {
                        var resultadoCondicion = new Rules_TipoCondicionInscripcion(getUsuarioLogueado()).GetByKeyValue(comando.TipoCondicionInscripcionKeyValue.Value);
                        if (!resultadoCondicion.Ok)
                        {
                            resultado.Error = resultadoCondicion.Error;
                            return false;
                        }
                        condicion = resultadoCondicion.Return;
                        if (condicion == null || condicion.FechaBaja != null)
                        {
                            resultado.Error = "La condicion de inscripción no existe o esta dada de baja";
                            return false;
                        }
                    }

                    //Valido las fechas
                    DateTime? fechaInicio = null;
                    DateTime? fechaFin = null;
                    DateTime? fechaTelegrama = null;
                    DateTime? rcondVce = null;
                    DateTime? artVce = null;

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

                    if (!string.IsNullOrEmpty(comando.FechaTelegrama))
                    {
                        fechaTelegrama = Utils.StringToDate(comando.FechaTelegrama);
                        if (fechaTelegrama == null)
                        {
                            resultado.Error = "El formato de la fecha de telegrama es inválida";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(comando.FechaVencimientoLicencia))
                    {
                        rcondVce = Utils.StringToDate(comando.FechaVencimientoLicencia);
                        if (rcondVce == null)
                        {
                            resultado.Error = "El formato de la fecha de RconVce es inválida";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(comando.ArtFechaVencimiento))
                    {
                        artVce = Utils.StringToDate(comando.ArtFechaVencimiento);
                        if (artVce == null)
                        {
                            resultado.Error = "El formato de la fecha de ArtVce es inválida";
                            return false;
                        }
                    }


                    //Creo la entidad
                    var entity = new Inscripcion()
                    {
                        TipoInscripcion = tipoInscripcion,
                        TipoAuto = tipoAuto,
                        Usuario = usuario,
                        Identificador = comando.Identificador,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        FechaTelegrama = fechaTelegrama,
                        FechaVencimientoLicencia = rcondVce,
                        ArtCompañia = comando.ArtCompañia,
                        ArtFechaVencimiento = artVce,
                        Caja = comando.Caja,
                        Observaciones = comando.Observaciones,
                        TipoCondicionInscripcion = condicion
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

        public Resultado<Inscripcion> Actualizar(_Model.Comandos.Comando_InscripcionActualizar comando)
        {
            var resultado = new Resultado<Inscripcion>();

            var resultadoTransaccion = dao.Transaction(() =>
            {
                try
                {
                    var validarComando = ValidarComandoActualizar(comando);
                    if (!validarComando.Ok)
                    {
                        resultado.Error = validarComando.Error;
                        return false;
                    }

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

                    //Busco el tipo inscripcion
                    var resultadoTipoInscripcion = new Rules_TipoInscripcion(getUsuarioLogueado()).GetByKeyValue(comando.TipoInscripcionKeyValue);
                    if (!resultadoTipoInscripcion.Ok)
                    {
                        resultado.Error = resultadoTipoInscripcion.Error;
                        return false;
                    }

                    var tipoInscripcion = resultadoTipoInscripcion.Return;
                    if (tipoInscripcion == null || tipoInscripcion.FechaBaja != null)
                    {
                        resultado.Error = "El tipo de inscripción no existe o esta dado de baja";
                        return false;
                    }

                    //Busco el tipo de auto
                    var resultadoTipoAuto = new Rules_TipoAuto(getUsuarioLogueado()).GetByKeyValue(comando.TipoAutoKeyValue);
                    if (!resultadoTipoAuto.Ok)
                    {
                        resultado.Error = resultadoTipoAuto.Error;
                        return false;
                    }
                    var tipoAuto = resultadoTipoAuto.Return;
                    if (tipoAuto == null || tipoAuto.FechaBaja != null)
                    {
                        resultado.Error = "El tipo de auto no existe o esta dado de baja";
                        return false;
                    }

                    //Busco el tipo condicion
                    TipoCondicionInscripcion condicion = null;
                    if (comando.TipoCondicionInscripcionKeyValue.HasValue)
                    {
                        var resultadoCondicion = new Rules_TipoCondicionInscripcion(getUsuarioLogueado()).GetByKeyValue(comando.TipoCondicionInscripcionKeyValue.Value);
                        if (!resultadoCondicion.Ok)
                        {
                            resultado.Error = resultadoCondicion.Error;
                            return false;
                        }
                        condicion = resultadoCondicion.Return;
                        if (condicion == null || condicion.FechaBaja != null)
                        {
                            resultado.Error = "La condicion de inscripción no existe o esta dada de baja";
                            return false;
                        }
                    }

                    //Valido las fechas
                    DateTime? fechaInicio = null;
                    DateTime? fechaFin = null;
                    DateTime? fechaTelegrama = null;
                    DateTime? fechaVencimientoLicencia = null;
                    DateTime? artFechaVencimiento = null;

                    if (!string.IsNullOrEmpty(comando.FechaInicio))
                    {
                        fechaInicio = Utils.StringToDate(comando.FechaInicio);
                        if (fechaInicio == null)
                        {
                            resultado.Error = "El formato de la fecha de inicio es inválido";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(comando.FechaFin))
                    {
                        fechaFin = Utils.StringToDate(comando.FechaFin);
                        if (fechaFin == null)
                        {
                            resultado.Error = "El formato de la fecha de inicio es inválido";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(comando.FechaTelegrama))
                    {
                        fechaTelegrama = Utils.StringToDate(comando.FechaTelegrama);
                        if (fechaTelegrama == null)
                        {
                            resultado.Error = "El formato de la fecha de telegrama es inválido";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(comando.FechaVencimientoLicencia))
                    {
                        fechaVencimientoLicencia = Utils.StringToDate(comando.FechaVencimientoLicencia);
                        if (fechaVencimientoLicencia == null)
                        {
                            resultado.Error = "El formato de la fecha de vencimiento de la licencia es inválido";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(comando.ArtFechaVencimiento))
                    {
                        artFechaVencimiento = Utils.StringToDate(comando.ArtFechaVencimiento);
                        if (artFechaVencimiento == null)
                        {
                            resultado.Error = "El formato de la fecha de vencimiento de la ART es inválido";
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
                    entity.Id = comando.Id;
                    entity.Identificador = comando.Identificador;
                    entity.Usuario = usuario;
                    entity.TipoInscripcion = tipoInscripcion;
                    entity.TipoAuto = tipoAuto;
                    entity.FechaInicio = fechaInicio;
                    entity.FechaFin = fechaFin;
                    entity.FechaTelegrama = fechaTelegrama;
                    entity.FechaVencimientoLicencia = fechaVencimientoLicencia;
                    entity.ArtCompañia = comando.ArtCompañia;
                    entity.ArtFechaVencimiento = artFechaVencimiento;
                    entity.Caja = comando.Caja;
                    entity.Observaciones = comando.Observaciones;
                    entity.TipoCondicionInscripcion = condicion;
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
                resultado.Error = "El inscripto no existe o esta dado de baja";
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

        public Resultado<bool> ValidarComandoActualizar(_Model.Comandos.Comando_InscripcionActualizar comando)
        {
            var resultado = new Resultado<bool>();

            try
            {
                if (comando.Id==0 || comando.Id<0)
                {
                    resultado.Error = "El Id de la inscripción a actualizar es requerido";
                    return resultado;
                }

                if (comando.IdUsuario == 0 || comando.IdUsuario < 0)
                {
                    resultado.Error = "El Id Usuario es requerido";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.TipoAuto), comando.TipoAutoKeyValue))
                {
                    resultado.Error = "El identificador del Tipo Auto esta fuera de rango";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.TipoInscripcion), comando.TipoInscripcionKeyValue))
                {
                    resultado.Error = "El identificador del Tipo Inscripción esta fuera de rango";
                    return resultado;
                }
                
                if (string.IsNullOrEmpty(comando.Identificador))
                {
                    resultado.Error = "El campo Identificador es requerido";
                    return resultado;
                }

                if (string.IsNullOrEmpty(comando.FechaInicio))
                {
                    resultado.Error = "El campo Fecha Inicio es requerido";
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

        public Resultado<bool> ValidarComandoInsertar(_Model.Comandos.Comando_InscripcionNuevo comando)
        {
            var resultado = new Resultado<bool>();

            try
            {             
                if (string.IsNullOrEmpty(comando.Identificador))
                {
                    resultado.Error = "El campo Identificador es requerido";
                    return resultado;
                }

                if (comando.IdUsuario == 0 || comando.IdUsuario < 0)
                {
                    resultado.Error = "El Id Usuario es requerido";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.TipoAuto), comando.TipoAutoKeyValue))
                {
                    resultado.Error = "El identificador del Tipo Auto esta fuera de rango";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.TipoInscripcion), comando.TipoInscripcionKeyValue))
                {
                    resultado.Error = "El identificador del Tipo Inscripción esta fuera de rango";
                    return resultado;
                }

                if (string.IsNullOrEmpty(comando.FechaInicio))
                {
                    resultado.Error = "El campo Fecha Inicio es requerido";
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