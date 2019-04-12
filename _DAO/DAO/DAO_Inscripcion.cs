using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;
using _Model.Resultados;
using NHibernate.Criterion.Lambda;

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

        public IQueryOver<Inscripcion, Inscripcion> GetQuery(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var query = GetSession().QueryOver<Inscripcion>();


            if (consulta.Dni.HasValue)
            {
                var joinUsuario = query.JoinQueryOver<Usuario>(x => x.Usuario);
                joinUsuario.Where(x => x.Dni == consulta.Dni.Value);
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
                query = query.Where(x => x.Identificador == consulta.Identificador);
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
                            var joinTipo = query.JoinQueryOver<TipoInscripcion>(x => x.TipoInscripcion);
                            var orderBy = joinTipo.OrderBy(x => x.Nombre);
                            var q = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;

                    case Enums.InscripcionOrderBy.TipoAuto:
                        {
                            var joinTipo = query.JoinQueryOver<TipoAuto>(x => x.TipoAuto);
                            var orderBy = joinTipo.OrderBy(x => x.Nombre);
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
                    case Enums.InscripcionOrderBy.FechaTelegrama:
                        {
                            var orderBy = query.OrderBy(x => (x.FechaTelegrama));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InscripcionOrderBy.FechaVencimientoLicencia:
                        {
                            var orderBy = query.OrderBy(x => (x.FechaVencimientoLicencia));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InscripcionOrderBy.ArtCompañia:
                        {
                            var orderBy = query.OrderBy(x => (x.ArtCompañia));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InscripcionOrderBy.ArtFechaVencimiento:
                        {
                            var orderBy = query.OrderBy(x => (x.ArtFechaVencimiento));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InscripcionOrderBy.Caja:
                        {
                            var orderBy = query.OrderBy(x => (x.Caja));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InscripcionOrderBy.UsuarioApellidoNombre:
                        {
                            var join = query.JoinQueryOver<Usuario>(x => x.Usuario);

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

        public Resultado<bool> CorregirFechaInicio()
        {
            var resultado = new Resultado<bool>();


            try
            {

                using (var s = SessionManager.Instance.SessionFactory.OpenSession())
                {
                    s.SetBatchSize(1000);

                    var usuarios = s.QueryOver<Usuario>()
                        .List()
                        .ToList();

                    using (var t = s.BeginTransaction())
                    {
                        usuarios.ForEach((entity) =>
                        {
                            try
                            {
                                List<string> errores = new List<string>();

                                //Nombre
                                bool conNombre = entity.Nombre != null && entity.Nombre.Trim() != "";
                                bool conApellido = entity.Apellido != null && entity.Apellido.Trim() != "";
                                if (!conNombre && !conApellido)
                                {
                                    errores.Add("Sin nombre o apellido");
                                }

                                //DNI
                                bool conDni = entity.Dni.HasValue;
                                bool dniValido = conDni && entity.Dni.Value > 0 && entity.Dni.Value < 200000000;
                                if (!dniValido)
                                {
                                    errores.Add("N° de DNI inválido");
                                }

                                if (errores.Count != 0)
                                {
                                    entity.Error = string.Join(" - ", errores);
                                }
                                else
                                {
                                    entity.Error = null;
                                }
                                s.Update(entity);
                            }
                            catch (Exception ex)
                            {
                                var e = ex;
                            }
                        });

                        t.Commit();
                    }
                }

            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;
        }

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