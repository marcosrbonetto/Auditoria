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

        public Resultado<Usuario> Insertar(_Model.Comandos.Comando_UsuarioNuevo comando)
        {
            var resultado = new Resultado<Usuario>();

            try
            {
                if (comando.Dni <= 0)
                {
                    resultado.Error = "Ingrese el dni";
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


                DateTime? fechaNacimiento = null;
                if (!string.IsNullOrEmpty(comando.FechaNacimiento))
                {
                    fechaNacimiento = Utils.StringToDate(comando.FechaNacimiento);
                    if (fechaNacimiento == null)
                    {
                        resultado.Error = "El formato de la fecha de nacimiento es inválida";
                        return resultado;
                    }
                }

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
                //Busco el usuario
                var resultadoQueryUsuario = GetById(comando.Id);
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

                DateTime? fechaNacimiento = null;
                if (!string.IsNullOrEmpty(comando.FechaNacimiento))
                {
                    fechaNacimiento = Utils.StringToDate(comando.FechaNacimiento);
                    if (fechaNacimiento == null)
                    {
                        resultado.Error = "El formato de la fecha de nacimiento es inválida";
                        return resultado;
                    }
                }

                //Creo el usuario
                usuario.Dni = comando.Dni;
                usuario.SexoMasculino = comando.SexoMasculino;
                usuario.Nombre = comando.Nombre;
                usuario.Apellido = comando.Apellido;
                usuario.FechaNacimiento = fechaNacimiento;
                usuario.DomicilioBarrio = comando.DomicilioBarrio;
                usuario.DomicilioCalle = comando.DomicilioCalle;
                usuario.DomicilioAltura = comando.DomicilioAltura;
                usuario.DomicilioObservaciones = comando.DomicilioObservaciones;
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
    }
}