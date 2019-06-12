
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

        Dictionary<string, bool> joins = new Dictionary<string, bool>();
        Usuario joinUsuario;
        TipoInhabilitacion joinTipoInhabilitacion;

        private void ClearJoins()
        {
            joinTipoInhabilitacion = null;
            joinUsuario = null;
            joins = new Dictionary<string, bool>();
        }

        private void JoinUsuario(IQueryOver<Inhabilitacion, Inhabilitacion> query)
        {
            if (!joins.ContainsKey("usuario"))
            {
                query.Left.JoinAlias(x => x.Usuario, () => joinUsuario);
                joins["usuario"] = true;
            }
        }
        private void JoinTipoInhabilitacion(IQueryOver<Inhabilitacion, Inhabilitacion> query)
        {
            if (!joins.ContainsKey("tipoInhabilitacion"))
            {
                query.Left.JoinAlias(x => x.TipoInhabilitacion, () => joinTipoInhabilitacion);
                joins["tipoInhabilitacion"] = true;
            }
        }

        public IQueryOver<Inhabilitacion, Inhabilitacion> GetQuery(_Model.Consultas.Consulta_Inhabilitacion consulta)
        {

            ClearJoins();
            var query = GetSession().QueryOver<Inhabilitacion>();
            JoinTipoInhabilitacion(query);

            //Tipo
            if (consulta.TipoInhabilitacion.HasValue)
            {
                query = query.Where(() => joinTipoInhabilitacion.KeyValue == consulta.TipoInhabilitacion.Value);
            }

            //DNI
            if (consulta.Dni.HasValue)
            {
                JoinUsuario(query);
                query = query.Where(() => joinUsuario.Dni == consulta.Dni.Value);
            }

            //Nombre
            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                JoinUsuario(query);
                foreach (var palabra in consulta.Nombre.Split(' '))
                {
                    var p = palabra.Trim();
                    query.Where(() => (joinUsuario.Nombre != null && joinUsuario.Nombre.IsLike(p, MatchMode.Anywhere)) || (joinUsuario.Apellido != null && joinUsuario.Apellido.IsLike(p, MatchMode.Anywhere)));
                }
            }

            //Con error
            if (consulta.ConError.HasValue)
            {
                if (consulta.ConError.Value)
                {
                    query.Where(x => x.Error != null && x.Error != "");
                }
                else
                {
                    query.Where(x => x.Error == null || x.Error == "");
                }
            }

            //favorito
            if (consulta.Favorito.HasValue)
            {
                if (consulta.Favorito.Value)
                {
                    query.Where(x => x.Favorito == true);
                }
                else
                {
                    query.Where(x => x.Favorito == false);
                }
            }

            //Dado de baja
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
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy((x) => x.FechaInicio).Asc
                                    .ThenBy(x => x.Id).Asc;

                            }
                            else
                            {
                                query = query
                                    .OrderBy((x) => x.FechaInicio).Desc
                                    .ThenBy(x => x.Id).Desc;
                            }
                        } break;
                    case Enums.InhabilitacionOrderBy.FechaFin:
                        {
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy((x) => x.FechaFin).Asc
                                    .ThenBy(x => x.Id).Asc;
                            }
                            else
                            {
                                query = query
                                    .OrderBy((x) => x.FechaFin).Desc
                                    .ThenBy(x => x.Id).Desc;
                            }
                        } break;
                    case Enums.InhabilitacionOrderBy.UsuarioApellidoNombre:
                        {
                            JoinUsuario(query);
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy(() => joinUsuario.Apellido).Asc
                                    .ThenBy(() => joinUsuario.Nombre).Asc
                                    .ThenBy(x => x.Id).Asc;
                            }
                            else
                            {
                                query = query
                                    .OrderBy(() => joinUsuario.Apellido).Desc
                                    .ThenBy(() => joinUsuario.Nombre).Desc
                                    .ThenBy(x => x.Id).Desc;
                            }
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

        public Resultado<int> GetCantidadInhabilitaciones(_Model.Consultas.Consulta_Inhabilitacion consulta)
        {
            var resultado = new Resultado<int>();

            try
            {
                var query = GetQuery(consulta);

                var res = query.List().ToList();
                
                if (query.Clone().Where(x => !joinTipoInhabilitacion.Permanente && x.FechaInicio == null).RowCount() > 0)
                {
                    resultado.Error = "El sistema no puede corroborar la integridad de sus datos. Por favor, comuniquese con Dirección de Transporte";
                    return resultado;
                }

                //Inhabilitacion activa
                query = query.Where(x => joinTipoInhabilitacion.Permanente ||
                                        (x.FechaInicio.Value < DateTime.Now && x.FechaFin.Value > DateTime.Now) ||
                                        (x.FechaInicio.Value < DateTime.Now && x.FechaFin.Value == null));

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