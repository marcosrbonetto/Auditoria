using System;
using System.Linq;
using _DAO.DAO;
using _Model;
using _Model.Entities;
using System.Configuration;
using System.Collections.Generic;
using _Rules._WsVecinoVirtual;
using System.Text;

namespace _Rules.Rules
{
    public class Rules_Usuario : BaseRules<Usuario>
    {
        private readonly DAO_Usuario dao;


        public Rules_Usuario(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_Usuario.Instance;
        }

        public Resultado<_Model.Resultados.Resultado_Paginador<Usuario>> GetPaginado(_Model.Consultas.Consulta_UsuarioPaginado consulta)
        {
            return dao.GetPaginado(consulta);
        }

        public Resultado<List<Usuario>> Get(_Model.Consultas.Consulta_Usuario consulta)
        {
            return dao.Get(consulta);
        }
        public Resultado<int> GetCantidad(_Model.Consultas.Consulta_Usuario consulta)
        {
            return dao.GetCantidad(consulta);
        }

        public Resultado<Usuario> Insertar(_Model.Comandos.Comando_UsuarioNuevo comando)
        {
            var resultado = new Resultado<Usuario>();

            try
            {
                var validarComando = ValidarComandoInsertarActualizar(comando);
                if (!validarComando.Ok)
                {
                    resultado.Error = validarComando.Error;
                    return resultado;
                }

                //Busco en mi lista de usuario
                var resultadoQueryUsuario = Get(new _Model.Consultas.Consulta_Usuario()
                {
                    Dni = comando.Dni,
                    SexoMasculino = comando.SexoMasculino,
                    DadosDeBaja = false
                });
                if (!resultadoQueryUsuario.Ok)
                {
                    resultado.Error = resultadoQueryUsuario.Error;
                    return resultado;
                }
                bool existeUsuario = resultadoQueryUsuario.Return.Count != 0;
                if (existeUsuario)
                {
                    resultado.Error = "Ya existe un usuario con ese DNI y sexo";
                    return resultado;
                }

                DateTime? fechaNacimiento = Utils.StringToDate(comando.FechaNacimiento);

                //Creo el usuario
                Usuario usuario = new Usuario();
                usuario.Nombre = comando.Nombre;
                usuario.Apellido = comando.Apellido;
                usuario.Dni = comando.Dni;
                usuario.SexoMasculino = comando.SexoMasculino;
                usuario.FechaNacimiento = fechaNacimiento;
                usuario.DomicilioBarrio = comando.DomicilioBarrio;
                usuario.DomicilioCalle = comando.DomicilioCalle;
                usuario.DomicilioAltura = comando.DomicilioAltura;
                usuario.DomicilioObservaciones = comando.DomicilioObservaciones;
                usuario.DomicilioPiso = comando.DomicilioPiso;
                usuario.DomicilioDepto = comando.DomicilioDepto;
                usuario.DomicilioCodigoPostal = comando.DomicilioCodigoPostal;
                usuario.Observaciones = comando.Observaciones;

                //Inserto o update
                Resultado<Usuario> resultadoInsert = base.Insert(usuario);
                if (!resultadoInsert.Ok)
                {
                    resultado.Error = resultadoInsert.Error;
                    return resultado;
                }

                resultado.Return = resultadoInsert.Return;
                return resultado;
            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }
        }

        public Resultado<Usuario> Actualizar(_Model.Comandos.Comando_UsuarioActualizar comando)
        {
            var resultado = new Resultado<Usuario>();

            try
            {
                //var validarComando = ValidarComandoInsertarActualizar(comando);
                //if (!validarComando.Ok)
                //{
                //    resultado.Error = validarComando.Error;
                //    return resultado;
                //}


                if (!comando.Id.HasValue)
                {
                    resultado.Error = "Debe indicar el usuario a modificar";
                    return resultado;
                }

                //Busco el usuario
                var resultadoQueryUsuario = GetById(comando.Id.Value);
                if (!resultadoQueryUsuario.Ok)
                {
                    resultado.Error = resultadoQueryUsuario.Error;
                    return resultado;
                }

                var usuario = resultadoQueryUsuario.Return;
                if (usuario == null || usuario.FechaBaja != null)
                {
                    resultado.Error = "El usuario indicado no existe o está dado de baja";
                    return resultado;
                }

                //Busco en mi lista de usuario
                if (comando.Dni.HasValue && comando.SexoMasculino.HasValue)
                {
                    var resultadoQueryUsuarioExistente = Get(new _Model.Consultas.Consulta_Usuario()
                    {
                        Dni = comando.Dni,
                        SexoMasculino = comando.SexoMasculino,
                        DadosDeBaja = false
                    });
                    if (!resultadoQueryUsuarioExistente.Ok)
                    {
                        resultado.Error = resultadoQueryUsuario.Error;
                        return resultado;
                    }

                    bool existeUsuario = resultadoQueryUsuarioExistente.Return.Where(x => x.Id != comando.Id).ToList().Count != 0;
                    if (existeUsuario)
                    {
                        resultado.Error = "Ya existe un usuario con ese DNI y sexo";
                        return resultado;
                    }
                }


                //Valido fecha de nacimiento
                DateTime? fechaNacimiento = null;
                if (comando.FechaNacimiento != null && comando.FechaNacimiento.Trim() != "")
                {
                    fechaNacimiento = Utils.StringToDate(comando.FechaNacimiento);
                    if (!fechaNacimiento.HasValue)
                    {
                        resultado.Error = "Fecha de nacimiento inválida";
                    }
                }

                //Creo el usuario
                usuario.Id = comando.Id.Value;
                usuario.Dni = comando.Dni;
                usuario.SexoMasculino = comando.SexoMasculino;
                usuario.Nombre = comando.Nombre == null || comando.Nombre.Trim() == "" ? null : comando.Nombre.Trim();
                usuario.Apellido = comando.Apellido == null || comando.Apellido.Trim() == "" ? null : comando.Apellido.Trim();
                usuario.FechaNacimiento = fechaNacimiento;
                usuario.DomicilioBarrio = comando.DomicilioBarrio == null || comando.DomicilioBarrio.Trim() == "" ? null : comando.DomicilioBarrio.Trim();
                usuario.DomicilioCalle = comando.DomicilioCalle == null || comando.DomicilioCalle.Trim() == "" ? null : comando.DomicilioCalle.Trim();
                usuario.DomicilioAltura = comando.DomicilioAltura == null || comando.DomicilioAltura.Trim() == "" ? null : comando.DomicilioAltura.Trim();
                usuario.DomicilioObservaciones = comando.DomicilioObservaciones == null || comando.DomicilioObservaciones.Trim() == "" ? null : comando.DomicilioObservaciones.Trim();
                usuario.DomicilioPiso = comando.DomicilioPiso == null || comando.DomicilioPiso.Trim() == "" ? null : comando.DomicilioPiso.Trim();
                usuario.DomicilioDepto = comando.DomicilioDepto == null || comando.DomicilioDepto.Trim() == "" ? null : comando.DomicilioDepto.Trim();
                usuario.DomicilioCodigoPostal = comando.DomicilioCodigoPostal == null || comando.DomicilioCodigoPostal.Trim() == "" ? null : comando.DomicilioCodigoPostal.Trim();
                usuario.Observaciones = comando.Observaciones == null || comando.Observaciones.Trim() == "" ? null : comando.Observaciones.Trim();

                //Error
                var error = CalcularError(usuario);
                usuario.Error = error;

                //Inserto o update
                Resultado<Usuario> resultadoInsert = base.Update(usuario);
                if (!resultadoInsert.Ok)
                {
                    resultado.Error = resultadoInsert.Error;
                    return resultado;
                }

                resultado.Return = resultadoInsert.Return;
                return resultado;
            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }
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
                resultado.Error = "El usuario no existe o esta dado de baja";
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
                        resultado.Error = "El usuario no existe o esta dado de baja";
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

        public Resultado<bool> ValidarComandoInsertarActualizar(_Model.Comandos.Comando_UsuarioNuevo comando)
        {
            var resultado = new Resultado<bool>();

            try
            {
                //id entidad
                if (comando is _Model.Comandos.Comando_UsuarioActualizar)
                {
                    _Model.Comandos.Comando_UsuarioActualizar c = (_Model.Comandos.Comando_UsuarioActualizar)comando;
                    if (!c.Id.HasValue)
                    {
                        resultado.Error = "El id del usuario requerido";
                        return resultado;
                    }
                }

                //Nombre Apellido
                if ((comando.Nombre == null || comando.Nombre.Trim() == "") || (comando.Apellido == null || comando.Apellido.Trim() == ""))
                {
                    resultado.Error = "El nombre y apellido son requeridos";
                    return resultado;
                }

                //Sexo
                if (!comando.SexoMasculino.HasValue)
                {
                    resultado.Error = "El sexo es requerido";
                    return resultado;
                }

                //DNI
                if (!comando.Dni.HasValue || comando.Dni.Value <= 0 || comando.Dni >= 200000000)
                {
                    resultado.Error = "N° de DNI inválido";
                    return resultado;
                }

                //Si manda fecha de nacimiento, debe ser valida
                if (!string.IsNullOrEmpty(comando.FechaNacimiento) && !Utils.StringToDate(comando.FechaNacimiento).HasValue)
                {
                    resultado.Error = "Fecha de nacimiento inválida";
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

        public string CalcularError(Usuario usuario)
        {
            List<string> errores = new List<string>();

            //Nombre
            if ((usuario.Nombre == null || usuario.Nombre.Trim() == ""))
            {
                errores.Add("Nombre requerido");
            }

            //Apellido
            if ((usuario.Apellido == null || usuario.Apellido.Trim() == ""))
            {
                errores.Add("Apellido requerido");
            }

            //DNI
            if (!usuario.Dni.HasValue)
            {
                errores.Add("N° de Documento requerido");
            }
            else
            {
                if (usuario.Dni.Value <= 0 || usuario.Dni.Value > 200000000)
                {
                    errores.Add("N° de documento inválido");
                }
            }

            //Sexo
            if (!usuario.SexoMasculino.HasValue)
            {
                errores.Add("Sexo requerido");
            }

            if (errores.Count == 0) return null;
            return string.Join(" | ", errores);
        }

    }
}