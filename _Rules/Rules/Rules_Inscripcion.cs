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

                    //Busco el tipo inscripcion
                    var resultadoTipoInscripcion = new Rules_TipoInscripcion(getUsuarioLogueado()).GetByKeyValue(comando.TipoInscripcionKeyValue.Value);
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
                    var resultadoTipoAuto = new Rules_TipoAuto(getUsuarioLogueado()).GetByKeyValue(comando.TipoAutoKeyValue.Value);
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

                    //Fechas
                    DateTime? fechaInicio = Utils.StringToDate(comando.FechaInicio);
                    DateTime? fechaFin = Utils.StringToDate(comando.FechaFin);
                    DateTime? fechaTelegrama = Utils.StringToDate(comando.FechaTelegrama);
                    DateTime? rcondVce = Utils.StringToDate(comando.FechaVencimientoLicencia);
                    DateTime? artVce = Utils.StringToDate(comando.ArtFechaVencimiento);

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

                    //Busco el tipo inscripcion
                    var resultadoTipoInscripcion = new Rules_TipoInscripcion(getUsuarioLogueado()).GetByKeyValue(comando.TipoInscripcionKeyValue.Value);
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
                    var resultadoTipoAuto = new Rules_TipoAuto(getUsuarioLogueado()).GetByKeyValue(comando.TipoAutoKeyValue.Value);
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
                    DateTime? fechaInicio = Utils.StringToDate(comando.FechaInicio); ;
                    DateTime? fechaFin = Utils.StringToDate(comando.FechaFin); ;
                    DateTime? fechaTelegrama = Utils.StringToDate(comando.FechaTelegrama); ;
                    DateTime? fechaVencimientoLicencia = Utils.StringToDate(comando.FechaVencimientoLicencia); ;
                    DateTime? artFechaVencimiento = Utils.StringToDate(comando.ArtFechaVencimiento); ;

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
                    entity.Id = comando.Id.Value;
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

        public Resultado<bool> ValidarComandoInsertarActualizar(_Model.Comandos.Comando_InscripcionNuevo comando)
        {
            var resultado = new Resultado<bool>();

            try
            {

                //Si es actualizar, valido el id
                if (comando is _Model.Comandos.Comando_InscripcionActualizar)
                {
                    _Model.Comandos.Comando_InscripcionActualizar c = (_Model.Comandos.Comando_InscripcionActualizar)comando;
                    if (!c.Id.HasValue || c.Id.Value <= 0)
                    {
                        resultado.Error = "El id de la inscripcion es requerido";
                        return resultado;
                    }
                }

                //Identificador requerdio
                if (string.IsNullOrEmpty(comando.Identificador))
                {
                    resultado.Error = "El identificador es requerido";
                    return resultado;
                }

                //Id usuario requerido
                if (!comando.IdUsuario.HasValue || comando.IdUsuario <= 0)
                {
                    resultado.Error = "El usuario es requerido";
                    return resultado;
                }

                //Tipo de auto requerido
                if (!comando.TipoAutoKeyValue.HasValue)
                {
                    resultado.Error = "El tipo de auto es requerido";
                    return resultado;
                }

                //Tipo de auto valido
                if (!Enum.IsDefined(typeof(_Model.Enums.TipoAuto), comando.TipoAutoKeyValue))
                {
                    resultado.Error = "Tipo de auto inválido";
                    return resultado;
                }

                //Si manda tipo inscripcion, debe ser valida
                if (comando.TipoInscripcionKeyValue.HasValue && !Enum.IsDefined(typeof(_Model.Enums.TipoInscripcion), comando.TipoInscripcionKeyValue))
                {
                    resultado.Error = "Tipo de inscripción inválido";
                    return resultado;
                }

                //FEcha de inicio requerida
                if (string.IsNullOrEmpty(comando.FechaInicio))
                {
                    resultado.Error = "La fecha de inicio es requerida";
                    return resultado;
                }

                //Fecha de inicio valida
                if (!Utils.StringToDate(comando.FechaInicio).HasValue)
                {
                    resultado.Error = "Fecha de inicio inválida";
                    return resultado;
                }

                //Si manda fecha de fin, debe ser valida
                if (!string.IsNullOrEmpty(comando.FechaFin) && !Utils.StringToDate(comando.FechaFin).HasValue)
                {
                    resultado.Error = "Fecha de fin inválida";
                    return resultado;
                }

                //Si manda fecha de telegrama, debe ser valida
                if (!string.IsNullOrEmpty(comando.FechaTelegrama) && !Utils.StringToDate(comando.FechaTelegrama).HasValue)
                {
                    resultado.Error = "Fecha de telegrama inválida";
                    return resultado;
                }

                //Si manda fecha de vencimiento de licencia, debe ser valida
                if (!string.IsNullOrEmpty(comando.FechaVencimientoLicencia) && !Utils.StringToDate(comando.FechaVencimientoLicencia).HasValue)
                {
                    resultado.Error = "Fecha de vencimiento de licencia inválida";
                    return resultado;
                }

                //Si manda fecha de vencimiento de ART, debe ser valida
                if (!string.IsNullOrEmpty(comando.ArtFechaVencimiento) && !Utils.StringToDate(comando.ArtFechaVencimiento).HasValue)
                {
                    resultado.Error = "Fecha de vencimiento de ART inválida";
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