
using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;
using _Model.Resultados;
using NHibernate.Criterion;

namespace _DAO.DAO
{
    public class DAO_Inhabilitacion : BaseDAO<Inhabilitacion>
    {
        private static DAO_Inhabilitacion instance;

        public static DAO_Inhabilitacion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_Inhabilitacion();
                }
                return instance;
            }
        }

        IQueryOver<Inhabilitacion, Usuario> joinUsuario;

        public IQueryOver<Inhabilitacion, Inhabilitacion> GetQuery(_Model.Consultas.Consulta_Inhabilitacion consulta)
        {
            joinUsuario = null;

            var query = GetSession().QueryOver<Inhabilitacion>();
            joinUsuario = query.JoinQueryOver<Usuario>(x => x.Usuario);

            if (consulta.Dni.HasValue)
            {
                joinUsuario.Where(x => x.Dni == consulta.Dni.Value);
            }

            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                foreach (var palabra in consulta.Nombre.Split(' '))
                {
                    var p = palabra.Trim();
                    joinUsuario.Where(x => x.Nombre.IsLike(p, MatchMode.Anywhere) || x.Apellido.IsLike(p, MatchMode.Anywhere));
                }
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

        public Resultado<List<Inhabilitacion>> Get(_Model.Consultas.Consulta_Inhabilitacion consulta)
        {
            var resultado = new Resultado<List<Inhabilitacion>>();

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

        public Resultado<int> GetCantidad(_Model.Consultas.Consulta_Inhabilitacion consulta)
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

        public Resultado<Resultado_Paginador<Inhabilitacion>> GetPaginado(_Model.Consultas.Consulta_InhabilitacionPaginada consulta)
        {
            var resultado = new Resultado<Resultado_Paginador<Inhabilitacion>>();

            try
            {
                int tamaño = consulta.TamañoPagina;
                if (tamaño == 0)
                {
                    resultado.Error = "Tamaño de página inválido";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.InhabilitacionOrderBy), consulta.OrderBy))
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
                    case Enums.InhabilitacionOrderBy.FechaInicio:
                        {
                            var orderBy = query.OrderBy(x => (x.FechaInicio));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InhabilitacionOrderBy.FechaFin:
                        {
                            var orderBy = query.OrderBy(x => (x.FechaFin));
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                    case Enums.InhabilitacionOrderBy.UsuarioApellidoNombre:
                        {
                            IQueryOver<Inhabilitacion, Usuario> join;
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

                resultado.Return = new Resultado_Paginador<Inhabilitacion>();
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


    }
}