﻿using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;
using _Model.Resultados;
using NHibernate.Criterion.Lambda;
using NHibernate.Criterion;

namespace _DAO.DAO
{
    public class DAO_Inscripcion : BaseDAO<Inscripcion>
    {
        private static DAO_Inscripcion instance;

        public static DAO_Inscripcion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_Inscripcion();
                }
                return instance;
            }
        }

        IQueryOver<Inscripcion, Usuario> joinUsuario;
        IQueryOver<Inscripcion, TipoInscripcion> joinTipoInscripcion;
        IQueryOver<Inscripcion, TipoAuto> joinTipoAuto;

        public IQueryOver<Inscripcion, Inscripcion> GetQuery(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            joinUsuario = null;
            joinTipoAuto = null;
            joinTipoInscripcion = null;

            var query = GetSession().QueryOver<Inscripcion>();
            joinUsuario = query.JoinQueryOver<Usuario>(x => x.Usuario);

            if (consulta.Dni.HasValue)
            {
                joinUsuario.Where(x => x.Dni == consulta.Dni.Value);
            }


            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                foreach(var palabra in consulta.Nombre.Split(' ')){
                    var p = palabra.Trim();
                    joinUsuario.Where(x => x.Nombre.IsLike(p, MatchMode.Anywhere) || x.Apellido.IsLike(p, MatchMode.Anywhere));
                }
            }

            if (consulta.ConFechaInicio.HasValue)
            {
                if (consulta.ConFechaInicio.Value)
                {
                    query = query.Where(x => x.FechaInicio != null);
                }
                else
                {
                    query = query.Where(x => x.FechaInicio == null);
                }
            }

            if (!string.IsNullOrEmpty(consulta.Identificador))
            {
                query = query.Where(x => x.Identificador.IsLike(consulta.Identificador, MatchMode.Anywhere));
            }

            if (consulta.DadosDeBaja.HasValue)
            {
                if (consulta.DadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }
            }

            return query;
        }

        public Resultado<List<Inscripcion>> Get(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var resultado = new Resultado<List<Inscripcion>>();

            try
            {
                var query = GetQuery(consulta);
                resultado.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;

        }

        public Resultado<Resultado_Paginador<Inscripcion>> GetPaginado(_Model.Consultas.Consulta_InscripcionPaginada consulta)
        {
            var resultado = new Resultado<Resultado_Paginador<Inscripcion>>();

            try
            {
                int tamaño = consulta.TamañoPagina;
                if (tamaño == 0)
                {
                    resultado.Error = "Tamaño de página inválido";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.InscripcionOrderBy), consulta.OrderBy))
                {
                    resultado.Error = "Debe enviar el ordenamiento deseado";
                    return resultado;
                }

                int pagina = consulta.Pagina;

                int count = GetQuery(consulta).RowCount();
                int cantidadPaginas = (int)Math.Ceiling((double)count / consulta.TamañoPagina);

                if (count != 0)
                {
                    if (pagina > cantidadPaginas || pagina < 0)
                    {
                        resultado.Error = "Página indicada inválida";
                        return resultado;
                    }
                }

                var query = GetQuery(consulta);

                switch (consulta.OrderBy)
                {
                    case Enums.InscripcionOrderBy.Identificador:
                        {
                            var orderBy = query.OrderBy(x => x.Identificador);
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;

                    case Enums.InscripcionOrderBy.TipoInscripcion:
                        {
                            IQueryOver<Inscripcion, TipoInscripcion> join;

                            if (joinTipoInscripcion == null)
                            {
                                join = query.JoinQueryOver<TipoInscripcion>(x => x.TipoInscripcion);
                            }
                            else
                            {
                                join = joinTipoInscripcion;
                            }

                            var orderBy = join.OrderBy(x => x.Nombre);
                            var q = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;

                    case Enums.InscripcionOrderBy.TipoAuto:
                        {
                            IQueryOver<Inscripcion, TipoAuto> join;

                            if (joinTipoInscripcion == null)
                            {
                                join = query.JoinQueryOver<TipoAuto>(x => x.TipoAuto);
                            }
                            else
                            {
                                join = joinTipoAuto;
                            }

                            var orderBy = join.OrderBy(x => x.Nombre);
                            var q = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InscripcionOrderBy.FechaInicio:
                        {
                            var orderBy = query.OrderBy(x => (x.FechaInicio));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InscripcionOrderBy.FechaFin:
                        {
                            var orderBy = query.OrderBy(x => (x.FechaFin));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InscripcionOrderBy.UsuarioApellidoNombre:
                        {
                            IQueryOver<Inscripcion, Usuario> join;

                            if (joinUsuario == null)
                            {
                                join = query.JoinQueryOver<Usuario>(x => x.Usuario);
                            }
                            else
                            {
                                join = joinUsuario;
                            }

                            var orderBy = join.OrderBy(x => x.Apellido);
                            var q = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                            orderBy = join.ThenBy(x => x.Nombre);
                            q = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;

                        } break;
                }


                var items = query.Take(tamaño).Skip(pagina * tamaño).List().ToList();

                resultado.Return = new Resultado_Paginador<Inscripcion>();
                resultado.Return.Count = count;
                resultado.Return.Data = items;
                resultado.Return.PaginaActual = consulta.Pagina;
                resultado.Return.TamañoPagina = consulta.TamañoPagina;
                resultado.Return.CantidadPaginas = cantidadPaginas;

                return resultado;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;

        }

        public Resultado<int> GetCantidad(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var resultado = new Resultado<int>();

            try
            {
                var query = GetQuery(consulta);
                resultado.Return = query.RowCount();
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;

        }



        //public Resultado<bool> GenerarErrorInhabilitacion()
        //{
        //    var resultado = new Resultado<bool>();

        //    try
        //    {
        //        using (var s = SessionManager.Instance.SessionFactory.OpenSession())
        //        {
        //            s.SetBatchSize(1000);

        //            var usuarios = s.QueryOver<Inhabilitacion>()
        //                .List()
        //                .ToList();

        //            using (var t = s.BeginTransaction())
        //            {
        //                usuarios.ForEach((entity) =>
        //                {
        //                    try
        //                    {
        //                        List<string> errores = new List<string>();

        //                        //Usuario
        //                        //Es error cuando no tiene usuario o cuando su usuario tiene error
        //                        bool conUsuario = entity.Usuario != null;
        //                        bool conUsuarioConError = entity.Usuario != null && entity.Usuario.Error != null;
        //                        if (!conUsuario || conUsuarioConError)
        //                        {
        //                            if (!conUsuario)
        //                            {
        //                                errores.Add("Sin usuario");
        //                            }
        //                            else
        //                            {
        //                                errores.Add("Usuario con error: " + entity.Usuario.Error);
        //                            }
        //                        }

        //                        //Tipo
        //                        //Cuando no tiene
        //                        //Cuando tiene pero es invalido
        //                        //Cuando tiene pero es permanente y no tiene fecha fin
        //                        bool conTipo= entity.TipoInhabilitacion!= null;
        //                        if (!conTipo)
        //                        {
        //                            errores.Add("Sin tipo inhabilitacion");
        //                        }
        //                        else
        //                        {
        //                            bool esValido = !entity.TipoInhabilitacion.Invalido;
        //                            if (!esValido)
        //                            {
        //                                errores.Add("Tipo inhabilitacion inválido");
        //                            }
        //                            else
        //                            {
        //                                //No es permanente y no tiene fecha fin
        //                                if (!entity.TipoInhabilitacion.Permanente && !entity.FechaFin.HasValue)
        //                                {
        //                                    errores.Add("Inhabilitacion no permanente y sin fecha de fin");
        //                                }
        //                            }
        //                        }



        //                        if (errores.Count != 0)
        //                        {
        //                            entity.Error = string.Join(" - ", errores);
        //                        }
        //                        else
        //                        {
        //                            entity.Error = null;
        //                        }
        //                        s.Update(entity);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        var e = ex;
        //                    }
        //                });

        //                t.Commit();
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resultado.SetError(e);
        //    }
        //    return resultado;
        //}


        //public Resultado<bool> GenerarErroresInscripcion()
        //{
        //    var resultado = new Resultado<bool>();

        //    try
        //    {
        //        using (var s = SessionManager.Instance.SessionFactory.OpenSession())
        //        {
        //            s.SetBatchSize(1000);

        //            var usuarios = s.QueryOver<Inscripcion>()
        //                .List()
        //                .ToList();

        //            using (var t = s.BeginTransaction())
        //            {
        //                usuarios.ForEach((entity) =>
        //                {
        //                    try
        //                    {
        //                        List<string> errores = new List<string>();

        //                        //Usuario
        //                        //Es error cuando no tiene usuario o cuando su usuario tiene error
        //                        bool conUsuario = entity.Usuario != null;
        //                        bool conUsuarioConError = entity.Usuario != null && entity.Usuario.Error != null;                                
        //                        if (!conUsuario || conUsuarioConError)
        //                        {
        //                            if (!conUsuario)
        //                            {
        //                                errores.Add("Sin usuario");
        //                            }
        //                            else
        //                            {
        //                                errores.Add("Usuario con error: " + entity.Usuario.Error);
        //                            }
        //                        }

        //                        //Tipo auto
        //                        //Cuando no tiene tipo de auto
        //                        bool conTipoAuto = entity.TipoAuto!=null;
        //                        if (!conTipoAuto)
        //                        {
        //                            errores.Add("Sin tipo de auto");
        //                        }

        //                        //Identificador
        //                        //Cuando no tiene identificador
        //                        bool conIdentificador = entity.Identificador != null && entity.Identificador.Trim() != "";
        //                        if (!conIdentificador)
        //                        {
        //                            errores.Add("Sin identificador");
        //                        }

        //                        if (errores.Count != 0)
        //                        {
        //                            entity.Error = string.Join(" - ", errores);
        //                        }
        //                        else
        //                        {
        //                            entity.Error = null;
        //                        }
        //                        s.Update(entity);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        var e = ex;
        //                    }
        //                });

        //                t.Commit();
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resultado.SetError(e);
        //    }
        //    return resultado;
        //}

        //public Resultado<bool> GenerarErroresUsuario()
        //{
        //    var resultado = new Resultado<bool>();

        //    try
        //    {
        //        using (var s = SessionManager.Instance.SessionFactory.OpenSession())
        //        {
        //            s.SetBatchSize(1000);

        //            var usuarios = s.QueryOver<Usuario>()
        //                .List()
        //                .ToList();

        //            using (var t = s.BeginTransaction())
        //            {
        //                usuarios.ForEach((entity) =>
        //                {
        //                    try
        //                    {
        //                        List<string> errores = new List<string>();

        //                        //Nombre
        //                        bool conNombre = entity.Nombre != null && entity.Nombre.Trim() != "";
        //                        bool conApellido = entity.Apellido != null && entity.Apellido.Trim() != "";
        //                        if (!conNombre && !conApellido)
        //                        {
        //                            errores.Add("El nombre y/o apellido es requerido");
        //                        }


        //                        //Sexo
        //                        if (!entity.SexoMasculino.HasValue)
        //                        {
        //                            errores.Add("El campo sexo es requerido");
        //                        }

        //                        //DNI
        //                        bool conDni = entity.Dni.HasValue;
        //                        bool dniValido = conDni && entity.Dni.Value > 0 && entity.Dni.Value < 200000000;
        //                        if (!dniValido)
        //                        {
        //                            errores.Add("El campo N° de DNI es inválido");
        //                        }

        //                        if (errores.Count != 0)
        //                        {
        //                            entity.Error = string.Join(" - ", errores);
        //                        }
        //                        else
        //                        {
        //                            entity.Error = null;
        //                        }
        //                        s.Update(entity);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        var e = ex;
        //                    }
        //                });

        //                t.Commit();
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resultado.SetError(e);
        //    }
        //    return resultado;
        //}

        //public Resultado<bool> CorregirFechaInicio()
        //{
        //    var resultado = new Resultado<bool>();


        //    try
        //    {
        //        using (var s = SessionManager.Instance.SessionFactory.OpenSession())
        //        {
        //            s.SetBatchSize(1000);

        //            var inscripciones = s.QueryOver<Inhabilitacion>()
        //                .Where(x => x.FechaInicioString != null)
        //                .List()
        //                .ToList();

        //            using (var t = s.BeginTransaction())
        //            {
        //                inscripciones.ForEach((entity) =>
        //                {
        //                    try
        //                    {
        //                        var fechaString = entity.FechaInicioString.Trim();
        //                        var partes = fechaString.Split('/');

        //                        var año = int.Parse(partes[2]);
        //                        //if (año <= 19)
        //                        //{
        //                        //    año += 2000;
        //                        //}
        //                        //else
        //                        //{
        //                        //    año += 1900;
        //                        //}

        //                        //int mes = -1;
        //                        //var mesString = partes[1].ToLower();
        //                        //if (mesString.Contains("ene")) mes = 1;
        //                        //if (mesString.Contains("feb")) mes = 2;
        //                        //if (mesString.Contains("mar")) mes = 3;
        //                        //if (mesString.Contains("abr")) mes = 4;
        //                        //if (mesString.Contains("may")) mes = 5;
        //                        //if (mesString.Contains("jun")) mes = 6;
        //                        //if (mesString.Contains("jul")) mes = 7;
        //                        //if (mesString.Contains("ago")) mes = 8;
        //                        //if (mesString.Contains("sep")) mes = 9;
        //                        //if (mesString.Contains("oct")) mes = 10;
        //                        //if (mesString.Contains("nov")) mes = 11;
        //                        //if (mesString.Contains("dic")) mes = 12;
        //                        int mes = int.Parse(partes[1].Trim().ToLower());

        //                        int dia = int.Parse(partes[0]);
        //                        if (mes <= 12 && año > 1900)
        //                        {
        //                            var fecha = new DateTime(año, mes, dia);
        //                            entity.FechaInicio = fecha;
        //                            entity.FechaInicioString = null;
        //                            s.Update(entity);
        //                        }

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        var e = ex;
        //                    }
        //                });

        //                t.Commit();
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resultado.SetError(e);
        //    }
        //    return resultado;
        //}
    }
}