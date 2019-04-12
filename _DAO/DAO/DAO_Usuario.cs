using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using _Model.Resultados;

namespace _DAO.DAO
{
    public class DAO_Usuario : BaseDAO<Usuario>
    {
        private static DAO_Usuario instance;

        public static DAO_Usuario Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_Usuario();
                }
                return instance;
            }
        }

        public IQueryOver<Usuario, Usuario> GetQuery(_Model.Consultas.Consulta_Usuario consulta)
        {
            var query = GetSession().QueryOver<Usuario>();

            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                query.Where(x => x.Nombre.IsLike(consulta.Nombre, MatchMode.Anywhere) || x.Apellido.IsLike(consulta.Nombre, MatchMode.Anywhere));
            }


            //Dni
            if (consulta.Dni.HasValue)
            {
                query.Where(x => x.Dni == consulta.Dni.Value);
            }

            //fecha Nacimiento
            if (consulta.FechaNacimiento.HasValue)
            {
                query.Where(x => x.FechaNacimiento.Value.Date == consulta.FechaNacimiento.Value.Date);
            }

            //Sexo
            if (consulta.SexoMasculino.HasValue)
            {
                query.Where(x => x.SexoMasculino == consulta.SexoMasculino.Value);
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

        public Resultado<List<Usuario>> Get(_Model.Consultas.Consulta_Usuario consulta)
        {
            var resultado = new Resultado<List<Usuario>>();

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

        public Resultado<Resultado_Paginador<Usuario>> GetPaginado(_Model.Consultas.Consulta_UsuarioPaginado consulta)
        {
            var resultado = new Resultado<Resultado_Paginador<Usuario>>();

            try
            {
                int tamaño = consulta.TamañoPagina;
                if (tamaño == 0)
                {
                    resultado.Error = "Tamaño de página inválido";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.UsuarioOrderBy), consulta.OrderBy))
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
                    case Enums.UsuarioOrderBy.Dni:
                        {
                            var orderBy = query.OrderBy(x => x.Dni);
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;

                    case Enums.UsuarioOrderBy.FechaNacimiento:
                        {
                            var orderBy = query.OrderBy(x => x.FechaNacimiento);
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;

                    case Enums.UsuarioOrderBy.Nombre:
                        {
                            var orderBy = query.OrderBy(x => x.Apellido);
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                            orderBy = query.ThenBy(x => x.Nombre);
                            query = consulta.OrderByAsc ? orderBy.Asc : orderBy.Desc;
                        } break;
                }


                var items = query.Take(tamaño).Skip(pagina * tamaño).List().ToList();

                resultado.Return = new Resultado_Paginador<Usuario>();
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

        public Resultado<int> GetCantidad(_Model.Consultas.Consulta_Usuario consulta)
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
    }
}