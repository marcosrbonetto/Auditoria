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
        private readonly Rules_Usuario _UsuarioRules;

        public Rules_Inscripcion(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_Inscripcion.Instance;
            _UsuarioRules = new Rules_Usuario(data);
        }

        public Resultado<_Model.Resultados.Resultado_Paginador<Inscripcion>> GetPaginado(_Model.Consultas.Consulta_InscripcionPaginada consulta)
        {
            return dao.GetPaginado(consulta);
        }

        public Resultado<List<Inscripcion>> Get(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            return dao.Get(consulta);
        }

        public Resultado<List<Inscripcion>> GetReporte(_Model.Enums.TipoAuto? tipoAuto, int? numero)
        {
            var chapa = numero.ToString();
            for (int i = 4 - chapa.Count(); i > 0; i--)
            {
                chapa = string.Format("{0}{1}", "0", chapa.ToString());
            }

            return dao.Get(new _Model.Consultas.Consulta_Inscripcion()
            {
                TipoAuto = tipoAuto,
                Identificador = chapa
            });
        }

        public Resultado<bool> EstaInscripto(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var result = new Resultado<bool>();

            if (!consulta.Dni.HasValue)
            {
                result.Error = "Dni requerido";
                return result;
            }

            if (!consulta.TipoInscripcion.HasValue || !Enum.IsDefined(typeof(_Model.Enums.TipoInscripcion), consulta.TipoInscripcion))
            {
                result.Error = "Tipo Inscripción requerida";
                return result;
            }

            var resultadoInscripciones = dao.GetCantidadInscripto(new _Model.Consultas.Consulta_Inscripcion
            {
                Dni = consulta.Dni,
                FechaReferencia = consulta.FechaReferencia,
                TipoInscripcion = consulta.TipoInscripcion,
                DadosDeBaja = null
            });
            if (!resultadoInscripciones.Ok)
            {
                result.Error = resultadoInscripciones.Error;
                return result;
            }

            result.Return = resultadoInscripciones.Return > 0 ? true : false;

            return result;
        }

        public Resultado<double> GetAntiguedadEnDias(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var result = new Resultado<double>();

            if (!consulta.Dni.HasValue)
            {
                result.Error = "Dni requerido";
                return result;
            }

            if (!consulta.TipoInscripcion.HasValue || !Enum.IsDefined(typeof(_Model.Enums.TipoInscripcion), consulta.TipoInscripcion))
            {
                result.Error = "Tipo Inscripción requerida";
                return result;
            }

            var resultadoInscripciones = dao.GetAntiguedadEnDias(new _Model.Consultas.Consulta_Inscripcion
            {
                Dni = consulta.Dni,
                FechaReferencia = consulta.FechaReferencia,
                TipoInscripcion = consulta.TipoInscripcion,
                DadosDeBaja = null
            });
            if (!resultadoInscripciones.Ok)
            {
                result.Error = resultadoInscripciones.Error;
                return result;
            }

            result.Return = resultadoInscripciones.Return;

            return result;
        }

        public Resultado<int> GetCantidad(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            return dao.GetCantidad(consulta);
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
                        resultado.Error = "La licencia no existe o esta dada de baja";
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

                    //Busco el tipo inscripcion
                    TipoInscripcion tipoInscripcion = null;
                    if (comando.TipoInscripcionKeyValue.HasValue)
                    {
                        var resultadoTipoInscripcion = new Rules_TipoInscripcion(getUsuarioLogueado()).GetByKeyValue(comando.TipoInscripcionKeyValue.Value);
                        if (!resultadoTipoInscripcion.Ok)
                        {
                            resultado.Error = resultadoTipoInscripcion.Error;
                            return false;
                        }

                        tipoInscripcion = resultadoTipoInscripcion.Return;
                        if (tipoInscripcion == null || tipoInscripcion.FechaBaja != null)
                        {
                            resultado.Error = "El tipo de inscripción no existe o esta dado de baja";
                            return false;
                        }
                    }

                    //Busco el tipo de auto
                    TipoAuto tipoAuto = null;
                    if (comando.TipoAutoKeyValue.HasValue)
                    {
                        var resultadoTipoAuto = new Rules_TipoAuto(getUsuarioLogueado()).GetByKeyValue(comando.TipoAutoKeyValue.Value);
                        if (!resultadoTipoAuto.Ok)
                        {
                            resultado.Error = resultadoTipoAuto.Error;
                            return false;
                        }
                        tipoAuto = resultadoTipoAuto.Return;
                        if (tipoAuto == null || tipoAuto.FechaBaja != null)
                        {
                            resultado.Error = "El tipo de auto no existe o esta dado de baja";
                            return false;
                        }
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

                    DateTime? fechaTelegrama = null;
                    if (comando.FechaTelegrama != null && comando.FechaTelegrama.Trim() != "")
                    {
                        fechaTelegrama = Utils.StringToDate(comando.FechaTelegrama.Trim());
                        if (!fechaTelegrama.HasValue)
                        {
                            resultado.Error = "Fecha de telegrama inválida";
                            return false;
                        }
                    }

                    DateTime? fechaVencimientoLicencia = null;
                    if (comando.FechaVencimientoLicencia != null && comando.FechaVencimientoLicencia.Trim() != "")
                    {
                        fechaVencimientoLicencia = Utils.StringToDate(comando.FechaVencimientoLicencia.Trim());
                        if (!fechaVencimientoLicencia.HasValue)
                        {
                            resultado.Error = "Fecha de vencimiento de la licencia inválida";
                            return false;
                        }
                    }

                    DateTime? artFechaVencimiento = null;
                    if (comando.ArtFechaVencimiento != null && comando.ArtFechaVencimiento.Trim() != "")
                    {
                        artFechaVencimiento = Utils.StringToDate(comando.ArtFechaVencimiento.Trim());
                        if (!artFechaVencimiento.HasValue)
                        {
                            resultado.Error = "Fecha de vencimiento de la licencia inválida";
                            return false;
                        }
                    }


                    //Actualizo
                    entity.Id = comando.Id.Value;
                    entity.Identificador = comando.Identificador == null || comando.Identificador.Trim() == "" ? null : comando.Identificador.Trim();
                    entity.Usuario = usuario;
                    entity.TipoInscripcion = tipoInscripcion;
                    entity.TipoAuto = tipoAuto;
                    entity.FechaInicio = fechaInicio;
                    entity.FechaFin = fechaFin;
                    entity.FechaTelegrama = fechaTelegrama;
                    entity.FechaVencimientoLicencia = fechaVencimientoLicencia;
                    entity.ArtCompañia = comando.ArtCompañia == null || comando.ArtCompañia.Trim() == "" ? null : comando.ArtCompañia.Trim();
                    entity.ArtFechaVencimiento = artFechaVencimiento;
                    entity.Caja = comando.Caja == null || comando.Caja.Trim() == "" ? null : comando.Caja.Trim();
                    entity.Observaciones = comando.Observaciones == null || comando.Observaciones.Trim() == "" ? null : comando.Observaciones.Trim();
                    entity.TipoCondicionInscripcion = condicion;

                    //Error
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
                        resultado.Error = "La licencia no existe o esta dada de baja";
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

        public Resultado<bool> ValidarComandoInsertar(_Model.Comandos.Comando_InscripcionNuevo comando)
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
                if (comando.Identificador == null || comando.Identificador.Trim() == "")
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
                if (comando.FechaInicio == null || comando.FechaInicio.Trim() == "")
                {
                    resultado.Error = "La fecha de inicio es requerida";
                    return resultado;
                }

                //Fecha de inicio valida
                if (!Utils.StringToDate(comando.FechaInicio.Trim()).HasValue)
                {
                    resultado.Error = "Fecha de inicio inválida";
                    return resultado;
                }

                //Si manda fecha de fin, debe ser valida
                if (!(comando.FechaFin == null || comando.FechaFin.Trim() == "") && !Utils.StringToDate(comando.FechaFin.Trim()).HasValue)
                {
                    resultado.Error = "Fecha de fin inválida";
                    return resultado;
                }

                //Si manda fecha de telegrama, debe ser valida
                if (!(comando.FechaTelegrama == null || comando.FechaTelegrama.Trim() == "") && !Utils.StringToDate(comando.FechaTelegrama.Trim()).HasValue)
                {
                    resultado.Error = "Fecha de telegrama inválida";
                    return resultado;
                }

                //Si manda fecha de vencimiento de licencia, debe ser valida
                if (!(comando.FechaVencimientoLicencia == null || comando.FechaVencimientoLicencia.Trim() == "") && !Utils.StringToDate(comando.FechaVencimientoLicencia.Trim()).HasValue)
                {
                    resultado.Error = "Fecha de vencimiento de licencia inválida";
                    return resultado;
                }

                //Si manda fecha de vencimiento de ART, debe ser valida
                if (!(comando.ArtFechaVencimiento == null || comando.ArtFechaVencimiento.Trim() == "") && !Utils.StringToDate(comando.ArtFechaVencimiento.Trim()).HasValue)
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

        public string CalcularError(Inscripcion entity)
        {
            List<string> errores = new List<string>();

            //Tipo
            if (entity.TipoInscripcion == null)
            {
                errores.Add("Tipo de licencia requerido");
            }

            //Auti
            if (entity.TipoAuto == null)
            {
                errores.Add("Tipo de auto requerido");
            }

            //identificador
            if (entity.Identificador == null || entity.Identificador.Trim() == "")
            {
                errores.Add("N° de licencia requerido");
            }

            ////fecha inicio
            //if (!entity.FechaInicio.HasValue)
            //{
            //    errores.Add("Fecha inicio requerido");
            //}

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

        public void calcularErrores()
        {
            var usr = dao.GenerarErroresUsuario();
            var insc = dao.GenerarErroresInscripcion();

        }
    }
}